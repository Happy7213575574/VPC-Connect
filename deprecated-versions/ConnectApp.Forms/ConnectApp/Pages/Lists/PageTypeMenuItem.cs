using System;
namespace ConnectApp.Pages.Lists
{
    public class PageTypeMenuItem
    {
        public PageTypeMenuItem(PageTypes type, string title)
        {
            PageType = type;
            Title = title;
        }

        public PageTypes PageType { get; private set; }
        public string Title { get; private set; }

        public string AutomationId { get { return "View: " + Title; } }

        public string AutomationName {  get { return "View: " + Title; } }
    }
}
