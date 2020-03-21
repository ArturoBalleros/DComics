using HtmlAgilityPack;
using ReadDCInfo.Models;
using ScrapySharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;

namespace ReadDCInfo.Services
{
    class ScrapService
    {
        public void scrapReadDC()
        {

            /*  foreach (CollectionWeb c in GetListUrlCollections(GetListUrlFilters()))
              {
                  Collection collection = new Collection();
                  collection.NameDC = c.Title;
                  if (c.Title.Contains("-)"))
                      collection.InProgress = true;                
              }*/


            GetIssues(@"https://www.readdc.com/Action-Comics-2016/comics-series/71637?ref=YnJvd3NlL3Nlcmllcy9kZXNrdG9wL2xpc3Qvc2VyaWVzTGlzdA&Issues_pg=");


        }
        private List<string> GetListUrlFilters()
        {
            List<string> listUrls = new List<string>();

            string url = @"https://www.readdc.com/browse-series?seriesList_alpha={0}&seriesList_pg=";
            listUrls.Add(string.Format(url, "%2523"));
            listUrls.Add(string.Format(url, "A"));
            listUrls.Add(string.Format(url, "B"));
            listUrls.Add(string.Format(url, "C"));
            listUrls.Add(string.Format(url, "D"));
            listUrls.Add(string.Format(url, "E"));
            listUrls.Add(string.Format(url, "F"));
            listUrls.Add(string.Format(url, "G"));
            listUrls.Add(string.Format(url, "H"));
            listUrls.Add(string.Format(url, "I"));
            listUrls.Add(string.Format(url, "J"));
            listUrls.Add(string.Format(url, "K"));
            listUrls.Add(string.Format(url, "L"));
            listUrls.Add(string.Format(url, "M"));
            listUrls.Add(string.Format(url, "N"));
            listUrls.Add(string.Format(url, "O"));
            listUrls.Add(string.Format(url, "P"));
            listUrls.Add(string.Format(url, "Q"));
            listUrls.Add(string.Format(url, "R"));
            listUrls.Add(string.Format(url, "S"));
            listUrls.Add(string.Format(url, "T"));
            listUrls.Add(string.Format(url, "U"));
            listUrls.Add(string.Format(url, "V"));
            listUrls.Add(string.Format(url, "W"));
            listUrls.Add(string.Format(url, "X"));
            listUrls.Add(string.Format(url, "Y"));
            listUrls.Add(string.Format(url, "Z"));

            /*try
            {
                HtmlWeb oWeb = new HtmlWeb();
                HtmlDocument doc = oWeb.Load(Url);
                //Coge la etiqueta, la convierte en lista y del elemento 0 coge los nodos hijos que tengan de nombre 'option'
                var nodeUrls = doc.DocumentNode.CssSelect(".alphabet-filter").ToList()[0].ChildNodes.Where(cn => cn.Name.Equals("option") && cn.Attributes.Any());
                //Recorro los nodos para extraer la url
                foreach (string s in nodeUrls.Select(node => node.Attributes.FirstOrDefault(la => la.Name.Equals("data-url")).Value).ToList())
                    listUrls.Add(string.Format("https://www.readdc.com{0}", s));
                listUrls.RemoveAt(0); //Es la de por defecto
            }
            catch (Exception)
            {
                listUrls = null;
            }*/

            return listUrls;
        }
        private List<CollectionWeb> GetListUrlCollections(List<string> urlLetters)
        {
            List<CollectionWeb> collectionsWeb = new List<CollectionWeb>();

            try
            {
                Console.WriteLine("Inicio");
                foreach (string url in urlLetters)
                {
                    int cont = 1; string currentUrl; bool exit = false;
                    while (true)
                    {
                        currentUrl = url + cont;
                        HtmlWeb oWeb = new HtmlWeb();
                        HtmlDocument doc = oWeb.Load(currentUrl);
                        cont++;
                        List<HtmlAttributeCollection> htmlAttributeCollections = doc.DocumentNode.CssSelect(".content-info").CssSelect(".content-details").Where(cn => cn.Attributes.Any()).Select(n => n.Attributes).ToList();
                        foreach (var attributes in htmlAttributeCollections)
                        {
                            CollectionWeb collectionWeb = new CollectionWeb();
                            collectionWeb.Title = attributes["title"].Value;
                            collectionWeb.UrlCollection = attributes["href"].Value;
                            collectionWeb.UrlListCollection = currentUrl;
                            //Si esta sales sino añades
                            if (collectionsWeb.Any(c => c.Title.Equals(collectionWeb.Title)))
                            {
                                exit = true; //Para salir del bucle de esa URL
                                break;
                            }
                            else
                                collectionsWeb.Add(collectionWeb);
                        }
                        if (exit) break;
                    }
                    Console.WriteLine(url);
                }
                Console.WriteLine("Fin");
            }
            catch (Exception)
            {
                collectionsWeb = null;
            }
            return collectionsWeb;
        }
        private List<Issue> GetIssues(string url)
        {
            List<Issue> issues = new List<Issue>();
            List<string> linksIssues = new List<string>();
            try
            {
                int cont = 1; string currentUrl; bool exit = false;
                while (true)
                {
                    currentUrl = url + cont;
                    HtmlWeb oWeb = new HtmlWeb();
                    HtmlDocument doc = oWeb.Load(currentUrl);
                    cont++;
                    List<string> urlsIssue = doc.DocumentNode.CssSelect(".Issues").CssSelect(".content-img-link").Select(a => a.Attributes.FirstOrDefault(la => la.Name.Equals("href")).Value).ToList();

                    foreach (string urlIssue in urlsIssue)
                        if (linksIssues.Any(s => s.Equals(urlIssue)))
                        {
                            exit = true; //Para salir del bucle de esa URL
                            break;
                        }
                        else
                            linksIssues.Add(urlIssue);
                    if (exit) break;
                }
                foreach (string link in linksIssues)
                    issues.Add(GetDataIssue(link));
            }
            catch (Exception)
            {
                issues = null;
            }
            return issues;
        }
        private Issue GetDataIssue(string url)
        {
            Issue issue = new Issue();
            try
            {
                bool isPages = false;
                bool isRelease = false;
                HtmlWeb oWeb = new HtmlWeb();
                HtmlDocument doc = oWeb.Load(url);

                issue.NameDC = doc.DocumentNode.CssSelect(".content_body_frame").CssSelect(".title").Where(x => x.Name.Equals("h1")).Select(x => x.InnerText).ToList()[0];
                issue.Number = issue.NameDC.Contains("#") ? Convert.ToInt32(issue.NameDC.Substring(issue.NameDC.IndexOf("#") + 1)) : -1;
                issue.Description = doc.DocumentNode.CssSelect(".item-description").Select(x => x.InnerText).ToList()[0].Replace("\r", string.Empty);
                issue.LinkImage = doc.DocumentNode.CssSelect(".detail-content").CssSelect(".cover").ToList()[0].Attributes.Where(at => at.Name.Equals("src")).ToList()[0].Value;
                DownloadImage(issue.LinkImage, issue.NameDC);

                foreach (var child in doc.DocumentNode.CssSelect(".credits").ToList()[0].ChildNodes)
                {
                    if (child.Name.Equals("h4") && child.InnerText.Equals("Page Count")) isPages = true;
                    if (child.Name.Equals("h4") && child.InnerText.Equals("Print Release Date")) isRelease = true;
                    if (child.Name.Equals("div") && isPages)
                    {
                        issue.PageCount = Convert.ToInt32(child.InnerText.Replace("Pages", string.Empty));
                        isPages = false;
                    }
                    if (child.Name.Equals("div") && isRelease)
                    {
                        issue.ReleaseDate = DateTime.Parse(child.InnerText);
                        isRelease = false;
                    }
                }
            }
            catch (Exception)
            {
                issue = null;
            }
            return issue;
        }

        private void DownloadImage(string url, string name)
        {
            try
            {
                WebClient client = new WebClient();
                Stream stream = client.OpenRead(url);
                Bitmap bitmap = new Bitmap(stream);
                stream.Flush();
                stream.Close();
                client.Dispose();
                bitmap.Save(name + ".jpeg", ImageFormat.Jpeg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
