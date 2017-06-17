using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CRMPortal.SharedServices.Helpers
{
    public static class DrownDownListHelper
    {
        /// <summary>
        /// Adds increased attributed functionality to DropDownListFor.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <typeparam name="T">The type of object in a list.</typeparam>
        /// <param name="htmlHelper">The object this method attaches to.</param>
        /// <param name="expression">The object the selected value binds to.</param>
        /// <param name="listModel">The Model object for creating this list.</param>
        /// <returns>MvcHtmlString - Html for Razor pages.</returns>
        public static MvcHtmlString DropDownListWithAttributesFor<TModel, TProperty, T>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, HtmlSelectListModel<T> listModel)
        {
            var dropdown = new TagBuilder("select");
            foreach (var selectAttribute in listModel.SelectAttributes)
            {
                dropdown.Attributes.Add(selectAttribute.Key, selectAttribute.Value);
            }
            string currentValue = null;
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (metadata != null && metadata.Model != null)
                currentValue = metadata.Model.ToString();

            var optionsBuilder = new StringBuilder();
            foreach (T item in listModel.DataObjects)
            {
                BuildOptionTags(listModel, optionsBuilder, item, currentValue);
            }
            if (string.IsNullOrWhiteSpace(currentValue) && listModel.ShouldUseEmptyValue)
            {
                optionsBuilder.Insert(0, "<option value=\"\">" + listModel.EmptyValueText + "</option>");
            }
            dropdown.InnerHtml = optionsBuilder.ToString();

            return new MvcHtmlString(dropdown.ToString(TagRenderMode.Normal));
        }

        internal static void BuildOptionTags<T>(HtmlSelectListModel<T> listModel, StringBuilder optionsBuilder, T item, string selectedValue)
        {
            var optionAttributes = GetOptionAttributes(listModel, item);
            string innerText = GetOptionInnerText(optionAttributes);

            if (optionsBuilder == null) { optionsBuilder = new StringBuilder(); }
            optionsBuilder.Append("<option ");
            bool defaultValueFound = false;
            foreach (var attribute in optionAttributes)
            {
                optionsBuilder.Append(string.Format("{0}=\"{1}\" ", attribute.Key, attribute.Value));
                if (attribute.Value == selectedValue)
                {
                    optionsBuilder.Append("selected=\"selected\" ");
                    defaultValueFound = true;
                }
            }
            optionsBuilder.Append(">" + innerText + "</option>");
        }

        internal static string GetOptionInnerText(IDictionary<string, string> optionAttributes)
        {
            string innerText;
            if (optionAttributes.TryGetValue("inner-text", out innerText))
            {
                optionAttributes.Remove("inner-text");
            }
            return innerText;
        }

        internal static Dictionary<string, string> GetOptionAttributes<T>(HtmlSelectListModel<T> listModel, T item)
        {
            var properties = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => listModel.OptionAttributes.Values.Contains(p.Name));
            var optionAttributes = new Dictionary<string, string>();
            foreach (var p in properties)
            {
                var p1 = p;
                foreach (var oa in listModel.OptionAttributes.Where(oa => p1.Name == oa.Value))
                {
                    optionAttributes.Add(oa.Key, p.GetValue(item, null).ToString());
                }
            }
            return optionAttributes;
        }
    }
}