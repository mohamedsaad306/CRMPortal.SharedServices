using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.Helpers
{
    public class HtmlSelectListModel<T>
    {
        /// <summary>
        /// That collection of data elements from which to create a the select options.
        /// </summary>
        public IEnumerable<T> DataObjects { get; set; }

        /// <summary>
        /// Often there is an option on top called "--Select item--" that has an empty value.
        /// </summary>
        public string EmptyValueText { get; set; }

        /// <summary>
        /// A bool value that checks for whether EmptyValueText is used or not
        /// </summary>
        public bool ShouldUseEmptyValue
        {
            get { return !string.IsNullOrWhiteSpace(EmptyValueText); }
        }

        /// <summary>
        /// This will add attributes to the <select></select> html tag.
        /// The key is the attribute, the value is the value.
        /// For example:
        ///     SelectAttributes.add("id", "select-id-1");
        /// Would result in this html:
        ///     <select id="select-id-1"></select>
        /// </summary>
        public Dictionary<string, string> SelectAttributes
        {
            get { return _SelectAttributes ?? (_SelectAttributes = new Dictionary<string, string>()); }
            set { _SelectAttributes = value; }
        } private Dictionary<string, string> _SelectAttributes;

        /// <summary>
        /// This will add attributes to the <option></option> html tag in a select list.
        /// The key is the attribute, the value is the property on the object that contains the value.
        /// A list of objects will be used to create the options. Using reflection, the property with a
        /// name matching the value will be found. If the value is "Country" then a property name Country
        /// will be found using reflection. 
        /// 
        /// Note: If inner-text is used it will not be an attribute but will be the text between the
        ///       opening and closing tags.
        /// 
        /// For example:
        ///     OptionAttributes.add("value", "CountryTwoLetter");
        ///     OptionAttributes.add("innter-text", "Country");
        /// If the the list had to objects and the Country values were "United States" and "Canada",
        /// and the CountryTwoLetter values were "US" and "CA", then the result would be this html:
        ///     <option value="US">United States</option>
        ///     <option value="US">United States</option>
        /// 
        /// Html data-[name] attributes are supported here as well.
        /// </summary>
        public Dictionary<string, string> OptionAttributes
        {
            get { return _OptionAttributes ?? (_OptionAttributes = new Dictionary<string, string>()); }
            set { _OptionAttributes = value; }
        } private Dictionary<string, string> _OptionAttributes;
    }
}