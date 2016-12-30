// Copyright (C) 2014 FoxTales
// Released under the MIT License
 
using System;
using System.Linq.Expressions;

namespace FoxTales.Infrastructure.ViewFramework
{
    public abstract class ModelConverterBase<TModel, TViewModel> where TViewModel : new()
    {
        /// <summary>
        /// Kombinere et kald til model mapperen og preprocessere derefter view modellen.
        /// </summary>
        /// <param name="model">Modellen der skal omdannes til en view model.</param>
        /// <returns>Den view model der er resultatet af mappingen.</returns>
        public TViewModel MapAndProcess(TModel model)
        {
            // ReSharper disable CompareNonConstrainedGenericWithNull - it's okay
            if (model == null) return new TViewModel();
            // ReSharper restore CompareNonConstrainedGenericWithNull

            var viewModel = GetModelToViewModelMapper().Compile().Invoke(model);
            PreProcessViewModels(viewModel);
            return viewModel;
        }

        /// <summary>
        /// Persisterer den bagvedliggende model i databasen hvis metoden overskrives. Standard implementeringen gør ingenting.
        /// </summary>
        /// <param name="viewModel">Den view model der skal persisteres.</param>
        public virtual void UpdateModel(TViewModel viewModel)
        { }

        /// <summary>
        /// Danner en expression der bruges af LINQ to SQL til at omdanne modellen til en view model.
        /// BEMÆRK: Her er man fuldstændig underlagt de regler der er for hvad LINQ to SQL kan klare. Dvs. at andre repository kald og lignende er banlyst.
        /// Ønsker man at lave nogle komplekse beregninger på view modellen skal dette gøres i PreProcessViewModels i stedet.
        /// Standard implementationen returnere en tom view model.
        /// </summary>
        /// <returns>En expression til konvertering.</returns>
        public virtual Expression<Func<TModel, TViewModel>> GetModelToViewModelMapper()
        {
            return e => new TViewModel();
        }

        /// <summary>
        /// Hvis denne metode overskrives kan den bruges til at lave ekstra beregninger efter view modellen er trukket ud fra persisteringslaget.
        /// Standard implementationen gør ingenting.
        /// </summary>
        /// <param name="viewModels">En collection af view models, der skal bearbejdes.</param>
        public virtual void PreProcessViewModels(params TViewModel[] viewModels)
        {
        }
    }
}
