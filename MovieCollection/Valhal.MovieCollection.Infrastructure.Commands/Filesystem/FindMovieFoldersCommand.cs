using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using Autofac;
using FoxTales.Infrastructure.CommandFramework;
using FoxTales.Infrastructure.Repository.EntityFramework6.Commands;
using Valhal.MovieCollection.DTO;
using Valhal.MovieCollection.DTO.Searchfolder;
using Valhal.MovieCollection.Infrastructure.Queries;
using Valhal.MovieCollection.Infrastructure.Servicebus;
using Valhal.MovieCollection.Infrastructure.Servicebus.Messages;
using Valhal.MovieCollection.Models.FileSystems;

namespace Valhal.MovieCollection.Infrastructure.Commands.Filesystem
{
    public class FindMovieFoldersCommand : CommandBase
    {
        private readonly SearchFolderDto _input;
        private readonly List<string> _movieExtensions = new List<string>() { "mkv", "avi", "mp4", "iso" };
        public FindMovieFoldersCommand(SearchFolderDto input) 
        {
            _input = input;
        }

        protected override void OnExecuting(ILifetimeScope lifetimeScope)
        {
            Console.WriteLine($"Searching path {_input.Path}");
            var bus = lifetimeScope.Resolve<IValhalBus>();

            var enumerable = new FileSystemEnumerable(new DirectoryInfo(_input.Path), "*.*", SearchOption.AllDirectories, _input.Recursive ? 2 : 1).ToList();
            var casted = enumerable.OfType<FileInfo>().ToList();
            var grouped = casted.GroupBy(c => c.DirectoryName);
            foreach (var f in grouped)
            {
                var id = new MovieFolderExistsByPathQuery(f.Key).Execute(IsolationLevel.ReadCommitted).GetAll(c => c).FirstOrDefault();
                if (id) continue;//already exists
                var files = f.ToList();
                var movieFile = files.FirstOrDefault(c => _movieExtensions.Any(m => c.FullName.ToLower().EndsWith(m)));
                if (movieFile == null) continue;    //No movie file in dir
                var b = new MovieFolderAddMessage()
                {
                    DirectoryPath = f.Key,
                    Fanartpath = files.FirstOrDefault(c => c.FullName.EndsWith("fanart.jpg"))?.FullName,
                    Posterpath = files.FirstOrDefault(c => c.FullName.EndsWith("poster.jpg"))?.FullName,
                    Subtitlepath = files.FirstOrDefault(c => c.FullName.EndsWith("srt"))?.FullName,
                    MovieFilePath = movieFile?.FullName
                };
                var nfoFile = files.FirstOrDefault(c => c.FullName.EndsWith("movie.nfo"));//new NfoFile() { Path = f.FullName, Content = File.ReadAllText(f.FullName) };
                if (nfoFile != null)
                {
                    b.NfofilePath= nfoFile.FullName;
                    b.NfoContent = File.ReadAllText(nfoFile.FullName);
                };
                bus.Publish(b);

       //         StartChildCommand(new InsertCommand<MovieFolder, int>(b));
                Console.WriteLine($"\t{f.Key}");

            }
        }


        public class FileSystemEnumerable : IEnumerable<FileSystemInfo>
        {

            private readonly DirectoryInfo _root;
            private readonly IList<string> _patterns;
            private readonly SearchOption _option;
            private readonly int _maxrecursive;
            private int _current;

            public FileSystemEnumerable(DirectoryInfo root, string pattern, SearchOption option, int maxrecursive, int current = 0)
            {

                _root = root;
                _patterns = new List<string> { pattern };
                _option = option;
                _maxrecursive = maxrecursive;
                _current = current;
            }

            public FileSystemEnumerable(DirectoryInfo root, IList<string> patterns, SearchOption option, int maxrecursive, int current = 0)
            {
                _root = root;
                _patterns = patterns;
                _option = option;
                _maxrecursive = maxrecursive;
                _current = current;
            }

            public IEnumerator<FileSystemInfo> GetEnumerator()
            {
                if (_root == null || !_root.Exists) yield break;

                IEnumerable<FileSystemInfo> matches = new List<FileSystemInfo>();
                try
                {
                    foreach (var pattern in _patterns)
                    {
                        matches = matches.Concat(_root.EnumerateDirectories(pattern, SearchOption.TopDirectoryOnly))
                            .Concat(_root.EnumerateFiles(pattern, SearchOption.TopDirectoryOnly));
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    yield break;
                }
                catch (PathTooLongException)
                {
                    yield break;
                }
                catch (IOException)
                {
                    yield break;
                }

                foreach (var file in matches)
                {
                    yield return file;
                }

                if (_option == SearchOption.AllDirectories)
                {
                    foreach (var dir in _root.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
                    {
                        if (_current < _maxrecursive)
                        {
                            var fileSystemInfos = new FileSystemEnumerable(dir, _patterns, _option, _maxrecursive, _current + 1);
                            foreach (var match in fileSystemInfos)
                            {
                                yield return match;
                            }
                        }
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
