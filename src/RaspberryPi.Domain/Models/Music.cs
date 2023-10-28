namespace RaspberryPi.Domain.Models
{
    public class Music
    {
        public int[] Melody { get; set; }
        public int[] NoteDurations { get; set; }

        public Music(int[] melody, int[] noteDurations)
        {
            ArgumentNullException.ThrowIfNull(melody);
            ArgumentNullException.ThrowIfNull(noteDurations);

            if (melody.Count() != noteDurations.Count())
            {
                throw new ArgumentException("Melody and note durations must be the same length.");
            }

            Melody = melody;
            NoteDurations = noteDurations;
        }
    }
}