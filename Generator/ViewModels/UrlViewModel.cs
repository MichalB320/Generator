using CommunityToolkit.Mvvm.Input;
using Generator.Models;
using HtmlAgilityPack;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;

namespace Generator.ViewModels;

public class UrlViewModel : ViewModelBase
{
    private HtmlDocument _hmtlDocument;
    public ICommand FindCommand { get; }
    public ICommand OkCommand { get; }
    public ICommand FilterCommand { get; }

    private List<Node> _original;
    public ObservableCollection<Node> People { get; set; }

    private string _webData;
    public string WebData { get => _webData; set { _webData = value; OnPropertyChanged(nameof(WebData)); } }
    public string UrlInput { get; set; }
    public string Alias { get; set; }

    public List<List<string>> csv { get; set; }

    public UrlViewModel()
    {
        FindCommand = new RelayCommand(OnClickFind);
        OkCommand = new RelayCommand(OnCliclOk);
        _webData = "//a[starts-with(@name,'71')]";
        UrlInput = "https://vzdelavanie.uniza.sk/vzdelavanie/rozvrh2.php?sq=1&id=1002246";

        People = new();
        _original = new();
        FilterCommand = new RelayCommand(OnClickFilter);
        Alias = string.Empty;
        //csv = new();
    }

    private void OnClickFilter()
    {
        var filteredNodes = _hmtlDocument.DocumentNode.SelectNodes(WebData);
        People.Clear();
        foreach (var htmlNode in filteredNodes)
        {
            Node node = new();
            node.Tag = htmlNode.Name;

            if (htmlNode.HasAttributes)
            {
                foreach (var attribute in htmlNode.Attributes)
                {
                    string name = attribute.Name;

                    switch (name)
                    {
                        case "width":
                            node.Width = attribute.Value;
                            break;
                        case "type":
                            node.Type = attribute.Value;
                            break;
                        case "title":
                            node.Title = attribute.Value;
                            break;
                        case "target":
                            node.Target = attribute.Value;
                            break;
                        case "style":
                            node.Style = attribute.Value;
                            break;
                        case "src":
                            node.Src = attribute.Value;
                            break;
                        case "size":
                            node.Size = attribute.Value;
                            break;
                        case "selected":
                            node.Selected = attribute.Value;
                            break;
                        case "href":
                            node.Href = attribute.Value;
                            break;
                        case "heigh":
                            node.Heigh = attribute.Value;
                            break;
                        case "content":
                            node.Content = attribute.Value;
                            break;
                        case "rel":
                            node.Rel = attribute.Value;
                            break;
                        case "id":
                            node.Id = attribute.Value;
                            break;
                        case "class":
                            node.Class = attribute.Value;
                            break;
                        case "name":
                            node.Name = attribute.Value;
                            break;
                        case "alt":
                            node.Alt = attribute.Value;
                            break;
                        case "placeHolder":
                            node.PlaceHolder = attribute.Value;
                            break;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(htmlNode.InnerText))
            {
                node.Value = htmlNode.InnerHtml;
            }
            //_original.Add(node);
            People.Add(node);
        }
        //foreach (var item in _original)
        //    People.Add(item);
        //People = _original;
    }

    private void OnCliclOk()
    {

        //throw new NotImplementedException();
    }

    public async void OnClickFind()
    {
        using (HttpClient client = new())
        {
            try
            {
                string htmlContent = await client.GetStringAsync(UrlInput);

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlContent);

                HtmlNodeCollection allElements = htmlDocument.DocumentNode.SelectNodes("//*");

                //List<string> names = new();

                //foreach (HtmlNode element in allElements)
                //{
                //    foreach(HtmlAttribute attribute in element.Attributes)
                //    {
                //        //People.Add(attribute);
                //        names.Add(attribute.Name);
                //    }
                //}

                //foreach (HtmlNode element in allElements)
                //{
                //    foreach (HtmlAttribute attribute in element.Attributes)
                //    {
                //        People.Add(attribute);
                //    }
                //}

                foreach (var element in allElements)
                {
                    Node node = new();
                    node.Tag = element.Name.ToString();

                    //People.Add(node);

                    if (element.HasAttributes)
                    {

                        foreach (var attribute in element.Attributes)
                        {
                            string name = attribute.Name;
                            //if (name == "id" || name == "class" || name == "name" || name == "alt" || name == "placeHolder")
                            //    node.Name = attribute.Value;


                            //public string Rel { get; set; }
                            //public string Tag { get; set; }
                            switch (name)
                            {
                                case "width":
                                    node.Width = attribute.Value;
                                    break;
                                case "type":
                                    node.Type = attribute.Value;
                                    break;
                                case "title":
                                    node.Title = attribute.Value;
                                    break;
                                case "target":
                                    node.Target = attribute.Value;
                                    break;
                                case "style":
                                    node.Style = attribute.Value;
                                    break;
                                case "src":
                                    node.Src = attribute.Value;
                                    break;
                                case "size":
                                    node.Size = attribute.Value;
                                    break;
                                case "selected":
                                    node.Selected = attribute.Value;
                                    break;
                                case "href":
                                    node.Href = attribute.Value;
                                    break;
                                case "heigh":
                                    node.Heigh = attribute.Value;
                                    break;
                                case "content":
                                    node.Content = attribute.Value;
                                    break;
                                case "rel":
                                    node.Rel = attribute.Value;
                                    break;
                                case "id":
                                    node.Id = attribute.Value;
                                    break;
                                case "class":
                                    node.Class = attribute.Value;
                                    break;
                                case "name":
                                    node.Name = attribute.Value;
                                    break;
                                case "alt":
                                    node.Alt = attribute.Value;
                                    break;
                                case "placeHolder":
                                    node.PlaceHolder = attribute.Value;
                                    break;
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(element.InnerText))
                    {
                        node.Value = element.InnerHtml;
                    }
                    People.Add(node);
                }
                _hmtlDocument = htmlDocument;
                //WebData = sb.ToString();

                //var linksWithNamesStartingWith71 = htmlDocument.DocumentNode.SelectNodes("//a[starts-with(@name,'71')]");

                //var sb = new StringBuilder();
                //foreach (var name in linksWithNamesStartingWith71)
                //{
                //    sb.Append($"{name.InnerHtml}\n");
                //    People.Add(name.InnerHtml);
                //}
                //WebData = sb.ToString();
                //WebData = linksWithNamesStartingWith71[0].InnerHtml;

                //var value = htmlDocument.DocumentNode.SelectSingleNode("//a[@name='71264739']")?.InnerHtml;
                //WebData = value;
                //ExtractValues(htmlContent);

                //WebData = htmlContent;

            }
            catch (Exception e)
            {
                WebData = e.Message;
            }
        }
    }

    //private void ExtractValues(string content)
    //{
    //    int countS = content.Count(c => c == '>');
    //    int countE = content.Count(c => c == '<');
    //    WebData = $"strart {countS} end {countE}";

    //    int startI = content.IndexOf('>');
    //    //int count = _input.Count(c => c == '$');
    //    List<string> obsah = new();

    //    while(startI != -1)
    //    {
    //        int startIndex = startI;
    //        int endIndex = content.IndexOf('<', startIndex + 1);
    //        if (endIndex != -1)
    //        {
    //            //startI = content.IndexOf('$', endIndex + 1);
    //            string subStr = content.Substring(startIndex + 1, endIndex - 1 - startIndex);
    //            obsah.Add(subStr);
    //            startI = content.IndexOf('>', startIndex + 1);
    //        }
    //        else
    //        {
    //            startI = -1;
    //        }
    //    }

    //    var nieco = content.Split('>', '/');

    //    WebData = nieco[5];
    //}

    public void GetCSVFormat()
    {
        List<List<string>> rows = new();
        string[] titles = { "tag", "width", "type", "title", "target", "style", "src", "size", "selected", "href", "heigh", "content", "rel", "id", "class", "name", "alt", "placeHolder", "value" };
        List<string> titleRow = new();
        foreach (string title in titles)
        {
            titleRow.Add(title);
        }

        rows.Add(titleRow);

        foreach (var people in People)
        {
            List<string> row = new();
            row.Add(people.Tag);
            row.Add(people.Width);
            row.Add(people.Type);
            row.Add(people.Title);
            row.Add(people.Target);
            row.Add(people.Style);
            row.Add(people.Src);
            row.Add(people.Size);
            row.Add(people.Selected);
            row.Add(people.Href);
            row.Add(people.Heigh);
            row.Add(people.Content);
            row.Add(people.Rel);
            row.Add(people.Id);
            row.Add(people.Class);
            row.Add(people.Name);
            row.Add(people.Alt);
            row.Add(people.PlaceHolder);
            row.Add(people.Value);
            rows.Add(row);
        }

        csv = rows;

        //List<string> rows = new();

        //StringBuilder sb = new();

        //string titleRow = "width;type;title;target;style;src;size;selected;href;heigh;content;rel;id;class;name;alt;placeHolder;value;";
        //rows.Add(titleRow);
        //int index = 0;

        //sb.AppendLine(titleRow);

        //foreach (var people in People)
        //{
        //    string row = $"{people.Width};{people.Type};{people.Title};{people.Target};{people.Style};{people.Src};{people.Size};{people.Selected};{people.Href};{people.Heigh};{people.Content};{people.Rel};{people.Id};{people.Class};{people.Name};{people.Alt};{people.PlaceHolder};{people.Value};";
        //    rows.Add(row);
        //    sb.AppendLine(row);
        //    index++;
        //}
        //WebData = sb.ToString();
    }
}
