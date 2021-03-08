namespace quiet_hn
{
    public class HackerNewsEntry
    {
        public string by;
        public string decesdants;
        public int id;
        public int[] kids;
        public int score;
        public int time;
        public string title;
        public string type;
        public string url;

        public HackerNewsEntry(string title)
        //{ "by":"pg","descendants":15,"id":1,"kids":[15,234509,487171,454426,454424,454410,82729],"score":57,"time":1160418111,"title":"Y Combinator","type":"story","url":"http://ycombinator.com"}
        {
            this.title = title;
        }
    }
}
