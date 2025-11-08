namespace MauiCalc
{
    public class Cell1
    {
        public string Value { get; set; }
        public string Expression { get; set; }
        public List<string> linkedIn { get; set; }
        public List<string> linkInCell { get; set; }

        public Cell1()
        {
            Value = "";
            Expression = "";
            linkedIn = new List<string>();
            linkInCell = new List<string>();
        }
        public Cell1(string expression)
        {
            Value = "";
            Expression = expression;
            linkedIn = new List<string>();
            linkInCell = new List<string>();

        }
    }

}
