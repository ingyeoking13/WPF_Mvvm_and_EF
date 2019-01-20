using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using WPF_Mvvm_and_EF.Helper;

namespace WPF_Mvvm_and_EF.Wrapper
{
    public class ModelWrapper<T> : NotifyDataErrorBase
    {
        public T Model { get; set; }
        public ModelWrapper(T model)
        {
            Model = model;
        }

        protected virtual TValue GetValue<TValue>(
            [CallerMemberName]string propertyName=null)
        {
            return (TValue)(typeof(T).GetProperty(propertyName).GetValue(Model));
        }

        protected virtual void SetValue<TValue>(TValue value, 
            [CallerMemberName]string propertyName=null)
        {
            typeof(T).GetProperty(propertyName).SetValue(Model, value);
            OnPropertyChanged(propertyName);
            ValidatePropertyInternal(propertyName, value);
        }


        protected void ValidatePropertyInternal(string propertyName, 
            object currentValue)
        {
            ClearErrors(propertyName);
            ValidateDataAnnotation(propertyName, currentValue);
            ValidateCustomErrors(propertyName);
        }

        protected void ValidateDataAnnotation(string propertyName, 
            object currentValue)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(Model) { MemberName = propertyName };
            Validator.TryValidateProperty(currentValue, context, results);

            foreach (var result in results)
            {
                AddError(propertyName, result.ErrorMessage);
            }
        }

        protected void ValidateCustomErrors(string propertyName)
        {
            var errors = ValidateProperty(propertyName);
            if (errors != null)
            {
                foreach (var error in errors)
                {
                    AddError(propertyName, error);
                }
            }
        }

        protected virtual IEnumerable<string> ValidateProperty(string propertyName)
        {
            return null;
        }
    }

}
