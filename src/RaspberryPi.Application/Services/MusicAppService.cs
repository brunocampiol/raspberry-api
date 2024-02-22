using RaspberryPi.Application.Interfaces;
using RaspberryPi.Domain.Constants;
using RaspberryPi.Domain.Interfaces.Services;
using RaspberryPi.Domain.Models;
using RaspberryPi.Domain.Services;

namespace RaspberryPi.Application.Services
{
    public class MusicAppService : IMusicAppService
    {
        private readonly IBuzzerService _buzzerService;
        private bool _disposed;

        public MusicAppService(IBuzzerService buzzerService)
        {
            _buzzerService = buzzerService;
        }

        public void PlayMusic(Music music)
        {
            using var buzzer = new BuzzerService();
            for (int i = 0; i < music.Melody.Length; i++)
            {
                buzzer.PlayTone(music.Melody[i], music.NoteDurations[i]);
            }
        }

        public void PlayNokiaRingTone()
        {
            int tempo = 180;

            // notes of the moledy followed by the duration.
            // a 4 means a quarter note, 8 an eighteenth , 16 sixteenth, so on
            // !!negative numbers are used to represent dotted notes,
            // so -4 means a dotted quarter note, that is, a quarter plus an eighteenth!!
            int[] melody = {

              // Nokia Ringtone 
              // Score available at https://musescore.com/user/29944637/scores/5266155
  
              Note.E5, 8, Note.D5, 8, Note.FS4, 4, Note.GS4, 4,
              Note.CS5, 8, Note.B4, 8, Note.D4, 4, Note.E4, 4,
              Note.B4, 8, Note.A4, 8, Note.CS4, 4, Note.E4, 4,
              Note.A4, 2,
            };

            PlayMusicInternal(melody, tempo);
        }

        private void PlayMusicInternal(int[] melody, int tempo)
        {
            // sizeof gives the number of bytes, each int value is composed of two bytes (16 bits)
            // there are two values per note (pitch and duration), so for each note there are four bytes
            // int notes = sizeof(melody) / sizeof(melody[0]) / 2;
            int notes = melody.Length / 2;

            // this calculates the duration of a whole note in ms
            int wholenote = (60000 * 4) / tempo;

            int divider = 0, noteDuration = 0;

            using BuzzerService buzzer = new BuzzerService();

            // iterate over the notes of the melody. 
            // Remember, the array is twice the number of notes (notes + durations)
            for (int thisNote = 0; thisNote < notes * 2; thisNote = thisNote + 2)
            {

                // calculates the duration of each note
                divider = melody[thisNote + 1];
                if (divider > 0)
                {
                    // regular note, just proceed
                    noteDuration = (wholenote) / divider;
                }
                else if (divider < 0)
                {
                    // dotted notes are represented with negative durations!!
                    noteDuration = (wholenote) / Math.Abs(divider);
                    noteDuration *= 3 / 2; // increases the duration in half for dotted notes
                }

                // Wait for the specified duration before playing the next note.
                // We only play the note for 90% of the duration, leaving 10% as a pause
                buzzer.PlayTone(melody[thisNote], (int)(noteDuration * 0.9));
                //buzzer.PlayTone(melody[thisNote]);


                //Thread.Sleep((int)(noteDuration * 0.9));

                // stop the waveform generation before the next note.
                //Console.Beep(0, (int)(noteDuration * 0.1));
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _buzzerService.Dispose();
                }

                _disposed = true;
            }
        }

        ~MusicAppService()
        {
            Dispose(false);
        }
    }
}