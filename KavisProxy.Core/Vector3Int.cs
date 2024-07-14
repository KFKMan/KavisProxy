namespace KavisProxy.Core
{
    public class Chat
    {
        public Chat(string rawdata)
        {
            RawData = rawdata;
        }

        public string RawData;
    }

    public class Vector3Int
    {
        public Vector3Int(int x, int y, int z)
        {
            X = x;
            Y = y; 
            Z = z;
        }

        public int X;
        public int Y;
        public int Z;
    }
}