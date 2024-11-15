namespace GK1_MeshEditor
{
    public class DeltaTime
    {
        private DateTime time1;
        private DateTime time2;
        public static DeltaTime CreatePoint()
        {
            return new DeltaTime() { time1 = DateTime.Now, time2 = DateTime.Now };
        }
        public float GetDeltaTime()
        {
            time2 = DateTime.Now;
            float deltaTime = (time2.Ticks - time1.Ticks) / 10000000f;
            time1 = time2;
            return deltaTime;
        }
    }
}
