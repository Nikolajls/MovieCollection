using System;
using System.Data.SqlTypes;
using FluentValidation;
using FluentValidation.Validators;

namespace FoxTales.Infrastructure.DTOFramework.Extensions
{
    public static class DateTimeValidatorExtensions
    {
        public static IRuleBuilderOptions<T, DateTime?> IsValidSqlDateTime<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new SqlDateTimeNullableValidator());
        }

        public static IRuleBuilderOptions<T, DateTime> IsValidSqlDateTime<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new SqlDateTimeValidator());
        }
    }

    public class SqlDateTimeNullableValidator : PropertyValidator
    {
        public SqlDateTimeNullableValidator() : base("Property {PropertyName} is not a valid SQL Datetime.")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var dateTime = context.PropertyValue as DateTime?;
            if (dateTime != null)
            {
                return IsValidSqlDateTimeNative(dateTime);
            }
            return true;
        }

        /// <summary>
        ///     Validates if a date is valid as a SQL Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private bool IsValidSqlDateTimeNative(DateTime? date)
        {
            if (date == null) return true;
            var minValue = (DateTime) SqlDateTime.MinValue;
            var maxValue = (DateTime) SqlDateTime.MaxValue;

            if (minValue > date.Value || maxValue < date.Value)
            {
                return false;
            }

            return true;
        }
    }

    public class SqlDateTimeValidator : PropertyValidator
    {
        public SqlDateTimeValidator() : base("Property {PropertyName} is not a valid SQL Datetime.")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var dateTime = context.PropertyValue as DateTime?;
            if (dateTime == null) return false;
            return IsValidSqlDateTimeNative(dateTime);
        }

        /// <summary>
        ///     Validates if a date is valid as a SQL Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private bool IsValidSqlDateTimeNative(DateTime? date)
        {
            if (date == null) return false;
            var minValue = (DateTime) SqlDateTime.MinValue;
            var maxValue = (DateTime) SqlDateTime.MaxValue;

            if (minValue > date.Value || maxValue < date.Value)
            {
                return false;
            }

            return true;
        }
    }
}