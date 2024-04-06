using CommunityToolkit.Mvvm.Input;
using Generator.Models;
using HtmlAgilityPack;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace Generator.ViewModels;

public class UrlViewModel : ViewModelBase
{
    private HtmlDocument _hmtlDocument;
    public ICommand FindCommand { get; }
    public ICommand OkCommand { get; }
    public ICommand FilterCommand { get; }

    public ObservableCollection<Node> Nodes { get; set; } = new();

    private string _webData;
    public string WebData { get => _webData; set { _webData = value; OnPropertyChanged(nameof(WebData)); } }
    public string UrlInput { get; set; }
    public string Alias { get; set; } = string.Empty;
    public delegate void CloseWindowEventHandler();
    public event CloseWindowEventHandler? CloseWindowRequested;

    private bool _filterIsEnabled = false;
    public bool FilterIsEnabled { get => _filterIsEnabled; set { _filterIsEnabled = value; OnPropertyChanged(nameof(FilterIsEnabled)); } }

    public UrlViewModel()
    {
        FindCommand = new RelayCommand(OnClickFind);
        OkCommand = new RelayCommand(OnCliclOk);
        FilterCommand = new RelayCommand(OnClickFilter);
        _webData = "//a[starts-with(@name,'71')]";
        UrlInput = "https://vzdelavanie.uniza.sk/vzdelavanie/rozvrh2.php?sq=1&id=1002246";
    }

    private void OnClickFilter()
    {
        try
        {
            HtmlNodeCollection filteredNodes = _hmtlDocument.DocumentNode.SelectNodes(WebData);
            Nodes.Clear();
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
                    node.Value = htmlNode.InnerHtml;

                Nodes.Add(node);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void OnCliclOk()
    {
        CloseWindowRequested?.Invoke();
    }

    public async void OnClickFind()
    {
        using (HttpClient client = new())
        {
            try
            {
                string htmlContent = await client.GetStringAsync(UrlInput);

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlContent);

                HtmlNodeCollection allElements = htmlDocument.DocumentNode.SelectNodes("//*");

                foreach (var element in allElements)
                {
                    Node node = new();
                    node.Tag = element.Name.ToString();

                    if (element.HasAttributes)
                    {
                        foreach (var attribute in element.Attributes)
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

                    if (!string.IsNullOrWhiteSpace(element.InnerText))
                        node.Value = element.InnerHtml;

                    Nodes.Add(node);
                }
                _hmtlDocument = htmlDocument;

                FilterIsEnabled = true;
            }
            catch (Exception e)
            {
                WebData = e.Message;
            }
        }
    }

    public List<List<string>> GetCSVFormat()
    {
        List<List<string>> rows = new();
        string[] titles = { "tag", "width", "type", "title", "target", "style", "src", "size", "selected", "href", "heigh", "content", "rel", "id", "class", "name", "alt", "placeHolder", "value" };
        List<string> titleRow = new();
        foreach (string title in titles)
        {
            titleRow.Add(title);
        }

        rows.Add(titleRow);

        foreach (var people in Nodes)
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

        return rows;
    }
}
