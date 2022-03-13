using ManagedCommon;
using System.Windows;
using Wox.Plugin;

namespace Klaasie.Sf
{
    public class Main : IPlugin
    {
        private string? IconPath { get; set; }

        private PluginInitContext? Context { get; set; }

        public string Name => "SQL Formatter";

        public string Description => "Formats single line SQL strings to a readable format.";

        public void Init(PluginInitContext context)
        {
            Context = context;
            Context.API.ThemeChanged += OnThemeChanged;
            UpdateIconPath(Context.API.GetCurrentTheme());
        }

        public List<Result> Query(Query query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }


            var results = new List<Result>();

            if (!string.IsNullOrEmpty(query.ActionKeyword) && query.ActionKeyword == query.RawQuery)
            {
                results.Add(new Result
                {
                    Title = "select * from foo",
                    IcoPath = IconPath,
                    QueryTextDisplay = String.Empty,
                    Action = action =>
                    {
                        return true;
                    }
                });

                return results;
            }

            if (!string.IsNullOrEmpty(query.Search))
            {
                results.Add(new Result
                {
                    Title = "Format query",
                    SubTitle = query.Search,
                    IcoPath = IconPath,
                    QueryTextDisplay = String.Empty,
                    Action = action =>
                    {
                        Response response = Task.Run(async () => {
                            var result = await ApiClient.Format(query.Search);
                            return result;
                        }).Result;

                        Clipboard.SetText(response.Result);

                        return true;
                    }

                });

                return results;
            }

            return results;
        }

        private void UpdateIconPath(Theme theme)
        {
            if (theme == Theme.Light || theme == Theme.HighContrastWhite)
            {
                IconPath = "images/sf.light.png";
            }
            else
            {
                IconPath = "images/sf.dark.png";
            }
        }

        private void OnThemeChanged(Theme currentTheme, Theme newTheme)
        {
            UpdateIconPath(newTheme);
        }
    }
}