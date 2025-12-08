namespace Producer
{
    internal class Message : IMessage
    {

        private DateTime Time { get; }

        private int Counter;

        public Message()
        {
            DateTime currentTime = DateTime.Now;
            Time = currentTime;

            Counter = 0;
            
        }

        public void UpdateCounter()
        {
            Counter++;
        }

        public override string ToString()
        {
            return $"Message with counter at {Counter} written at ${Time}";
        }
    }
}
