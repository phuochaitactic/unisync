namespace BuildCongRenLuyen.Services
{
    public class IdGenerator
    {
        /// <summary>
        /// Gets the NewUID.
        /// </summary>
        public static long NewUID
        {
            get
            {
                return UIDToBig(Guid.NewGuid());
            }
        }

        public static long UIDToBig(Guid id)
        {
            var arr = id.ToByteArray();
            Array.Reverse(arr);
            var ff = BitConverter.ToInt32(arr, 0);
            if (ff != 0)
                return ff;
            return BitConverter.ToInt32(arr, 8);
        }
    }
}