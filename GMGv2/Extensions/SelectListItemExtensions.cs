using Microsoft.AspNetCore.Mvc.Rendering;

namespace GMGv2.Extensions
{
    public static class SelectListItemExtensions
    {
        extension<T>(IEnumerable<T> source)
        {
            public IEnumerable<SelectListItem> ToSelectListItems(Func<T, string> valueSelector, Func<T, string> textSelector)
            {
                return source.Select(item => new SelectListItem
                {
                    Value = valueSelector(item),
                    Text = textSelector(item)
                });
            }

            public List<SelectListItem> ToListSelectItems(Func<T, string> valueSelector, Func<T, string> textSelector)
            {
                return source.Select(item => new SelectListItem
                {
                    Value = valueSelector(item),
                    Text = textSelector(item)
                }).ToList();
            }
        }

        extension<T>(List<T> source)
        {
            public List<SelectListItem> ToSelectListItems(Func<T, string> valueSelector, Func<T, string> textSelector)
            {
                return source.Select(item => new SelectListItem
                {
                    Value = valueSelector(item),
                    Text = textSelector(item)
                }).ToList();
            }

            public IEnumerable<SelectListItem> ToEnumerableSelectListItems(Func<T, string> valueSelector, Func<T, string> textSelector)
            {
                return source.Select(item => new SelectListItem
                {
                    Value = valueSelector(item),
                    Text = textSelector(item)
                });
            }
        }
    }
}
