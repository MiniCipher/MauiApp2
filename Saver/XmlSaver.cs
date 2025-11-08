using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MauiCalc;

namespace Saver
{
    public class XmlSaver
    {
        /// <summary>
        /// Зберігає таблицю безпосередньо у файл.
        /// Це найбільш ефективний спосіб для роботи з диском.
        /// </summary>
        public void SaveToFile(string filePath, IDictionary<string, Cell1> cells, int rows, int columns)
        {
            var xDoc = CreateXDocument(cells, rows, columns);

            xDoc.Save(filePath);
        }

        /// <summary>
        /// Генерує XML у вигляді рядка (якщо тобі це потрібно для налагодження або передачі по мережі).
        /// </summary>
        public string GenerateContent(IDictionary<string, Cell1> cells, int rows, int columns)
        {
            var xDoc = CreateXDocument(cells, rows, columns);
            return xDoc.ToString();
        }

        /// <summary>
        /// Допоміжний приватний метод для створення структури XML.
        /// Використовує LINQ to XML для безпечної та чистої генерації.
        /// </summary>
        private XDocument CreateXDocument(IDictionary<string, Cell1> cells, int rows, int columns)
        {
            return new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("Cells",
                    new XAttribute("Rows", rows),
                    new XAttribute("Columns", columns),
                    // Використовуємо LINQ для перетворення словника Cells у XML елементи
                    from cellEntry in cells
                    let address = cellEntry.Key
                    let cell = cellEntry.Value
                    select new XElement("Cell",
                        new XElement("Address", address),
                        new XElement("Expression", cell.Expression),
                        new XElement("Value", cell.Value),
                        new XElement("LinkInCell",
                            // Важливо: використовуємо "Item", щоб відповідати ParseContent
                            from link in cell.linkInCell ?? new List<string>()
                            select new XElement("Item", link)
                        ),
                        new XElement("LinkedIn",
                            from link in cell.linkedIn ?? new List<string>()
                            select new XElement("Item", link)
                        )
                    )
                )
            );
        }

        public (IDictionary<string, Cell1> Cells, int Rows, int Columns) ParseContent(string xmlContent)
        {
            var cells = new Dictionary<string, Cell1>();
            int rows = 0;
            int columns = 0;

            // Якщо файл порожній або null, повертаємо порожній результат, щоб уникнути краху
            if (string.IsNullOrWhiteSpace(xmlContent))
            {
                return (cells, rows, columns);
            }

            var document = XDocument.Parse(xmlContent);
            var root = document.Root;

            if (root == null || root.Name != "Cells")
                throw new InvalidOperationException("Wrong XML file: Root element is not 'Cells'.");

            if (int.TryParse(root.Attribute("Rows")?.Value, out int parsedRows))
                rows = parsedRows;

            if (int.TryParse(root.Attribute("Columns")?.Value, out int parsedColumns))
                columns = parsedColumns;

            foreach (var cellElement in root.Elements("Cell"))
            {
                string address = cellElement.Element("Address")?.Value;

                // Якщо немає адреси — це пошкоджений запис, пропускаємо його
                if (string.IsNullOrEmpty(address)) continue;

                string value = cellElement.Element("Value")?.Value ?? "";
                string expression = cellElement.Element("Expression")?.Value ?? "";

                // Тут LINQ to XML автоматично знаходить всі дочірні елементи "Item"
                var linkedIn = cellElement.Element("LinkedIn")?.Elements("Item")
                                          .Select(x => x.Value).ToList() ?? new List<string>();
                var linkInCell = cellElement.Element("LinkInCell")?.Elements("Item")
                                           .Select(x => x.Value).ToList() ?? new List<string>();

                cells[address] = new Cell1
                {
                    Value = value,
                    Expression = expression,
                    linkedIn = linkedIn,
                    linkInCell = linkInCell
                };
            }

            return (cells, rows, columns);
        }
    }
}