using RaspberryPi.Domain.Constants;
using RaspberryPi.Domain.Interfaces.Services;
using RaspberryPi.Domain.Models;
using System.Security.Cryptography;

namespace RaspberryPi.Domain.Services
{
    public class MusicService
    {
        public MusicService(IBuzzerService buzzerService)
        {

        }

        public void PlayMusic(Music music)
        {
            using var buzzer = new BuzzerService();
            for (int i = 0; i < music.Melody.Length; i++)
            {
                buzzer.PlayTone(music.Melody[i], music.NoteDurations[i]);
            }
        }

        public void PlayImperialMarch()
        {
            /* 
                Imperial March - Star Wars
                More songs available at https://github.com/robsoncouto/arduino-songs                                            
                Robson Couto, 2019
            */

            // change this to make the song slower or faster
            int tempo = 120;

            // notes of the melody followed by the duration.
            // a 4 means a quarter note, 8 an eighteenth , 16 sixteenth, so on
            // !!negative numbers are used to represent dotted notes,
            // so -4 means a dotted quarter note, that is, a quarter plus an eighteenth!!
            int[] melody = {
                // Dart Vader theme (Imperial March) - Star wars 
                // Score available at https://musescore.com/user/202909/scores/1141521
                // The tenor saxophone part was used

                Note.A4,-4, Note.A4,-4, Note.A4,16, Note.A4,16, Note.A4,16, Note.A4,16, Note.F4,8, Note.REST,8,
                Note.A4,-4, Note.A4,-4, Note.A4,16, Note.A4,16, Note.A4,16, Note.A4,16, Note.F4,8, Note.REST,8,
                Note.A4,4, Note.A4,4, Note.A4,4, Note.F4,-8, Note.C5,16,

                Note.A4,4, Note.F4,-8, Note.C5,16, Note.A4,2,//4
                Note.E5,4, Note.E5,4, Note.E5,4, Note.F5,-8, Note.C5,16,
                Note.A4,4, Note.F4,-8, Note.C5,16, Note.A4,2,

                Note.A5,4, Note.A4,-8, Note.A4,16, Note.A5,4, Note.GS5,-8, Note.G5,16, //7 
                Note.DS5,16, Note.D5,16, Note.DS5,8, Note.REST,8, Note.A4,8, Note.DS5,4, Note.D5,-8, Note.CS5,16,

                Note.C5,16, Note.B4,16, Note.C5,16, Note.REST,8, Note.F4,8, Note.GS4,4, Note.F4,-8, Note.A4,-16,//9
                Note.C5,4, Note.A4,-8, Note.C5,16, Note.E5,2,

                Note.A5,4, Note.A4,-8, Note.A4,16, Note.A5,4, Note.GS5,-8, Note.G5,16, //7 
                Note.DS5,16, Note.D5,16, Note.DS5,8, Note.REST,8, Note.A4,8, Note.DS5,4, Note.D5,-8, Note.CS5,16,

                Note.C5,16, Note.B4,16, Note.C5,16, Note.REST,8, Note.F4,8, Note.GS4,4, Note.F4,-8, Note.A4,-16,//9
                Note.A4,4, Note.F4,-8, Note.C5,16, Note.A4,2,
            };

            PlayMusicInternal(melody, tempo);
        }

        public void PlaySuperMarioWorld()
        {
            // change this to make the song slower or faster
            int tempo = 200;
            int[] melody = {

              // Super Mario Bros theme
              // Score available at https://musescore.com/user/2123/scores/2145
              // Theme by Koji Kondo
  
              Note.E5,8, Note.E5,8, Note.REST,8, Note.E5,8, Note.REST,8, Note.C5,8, Note.E5,8, //1
              Note.G5,4, Note.REST,4, Note.G4,8, Note.REST,4,
              Note.C5,-4, Note.G4,8, Note.REST,4, Note.E4,-4, // 3
              Note.A4,4, Note.B4,4, Note.AS4,8, Note.A4,4,
              Note.G4,-8, Note.E5,-8, Note.G5,-8, Note.A5,4, Note.F5,8, Note.G5,8,
              Note.REST,8, Note.E5,4,Note.C5,8, Note.D5,8, Note.B4,-4,
              Note.C5,-4, Note.G4,8, Note.REST,4, Note.E4,-4, // repeats from 3
              Note.A4,4, Note.B4,4, Note.AS4,8, Note.A4,4,
              Note.G4,-8, Note.E5,-8, Note.G5,-8, Note.A5,4, Note.F5,8, Note.G5,8,
              Note.REST,8, Note.E5,4,Note.C5,8, Note.D5,8, Note.B4,-4,

              Note.REST,4, Note.G5,8, Note.FS5,8, Note.F5,8, Note.DS5,4, Note.E5,8,//7
              Note.REST,8, Note.GS4,8, Note.A4,8, Note.C4,8, Note.REST,8, Note.A4,8, Note.C5,8, Note.D5,8,
              Note.REST,4, Note.DS5,4, Note.REST,8, Note.D5,-4,
              Note.C5,2, Note.REST,2,

              Note.REST,4, Note.G5,8, Note.FS5,8, Note.F5,8, Note.DS5,4, Note.E5,8,//repeats from 7
              Note.REST,8, Note.GS4,8, Note.A4,8, Note.C4,8, Note.REST,8, Note.A4,8, Note.C5,8, Note.D5,8,
              Note.REST,4, Note.DS5,4, Note.REST,8, Note.D5,-4,
              Note.C5,2, Note.REST,2,

              Note.C5,8, Note.C5,4, Note.C5,8, Note.REST,8, Note.C5,8, Note.D5,4,//11
              Note.E5,8, Note.C5,4, Note.A4,8, Note.G4,2,

              Note.C5,8, Note.C5,4, Note.C5,8, Note.REST,8, Note.C5,8, Note.D5,8, Note.E5,8,//13
              Note.REST,1,
              Note.C5,8, Note.C5,4, Note.C5,8, Note.REST,8, Note.C5,8, Note.D5,4,
              Note.E5,8, Note.C5,4, Note.A4,8, Note.G4,2,
              Note.E5,8, Note.E5,8, Note.REST,8, Note.E5,8, Note.REST,8, Note.C5,8, Note.E5,4,
              Note.G5,4, Note.REST,4, Note.G4,4, Note.REST,4,
              Note.C5,-4, Note.G4,8, Note.REST,4, Note.E4,-4, // 19
  
              Note.A4,4, Note.B4,4, Note.AS4,8, Note.A4,4,
              Note.G4,-8, Note.E5,-8, Note.G5,-8, Note.A5,4, Note.F5,8, Note.G5,8,
              Note.REST,8, Note.E5,4, Note.C5,8, Note.D5,8, Note.B4,-4,

              Note.C5,-4, Note.G4,8, Note.REST,4, Note.E4,-4, // repeats from 19
              Note.A4,4, Note.B4,4, Note.AS4,8, Note.A4,4,
              Note.G4,-8, Note.E5,-8, Note.G5,-8, Note.A5,4, Note.F5,8, Note.G5,8,
              Note.REST,8, Note.E5,4, Note.C5,8, Note.D5,8, Note.B4,-4,

              Note.E5,8, Note.C5,4, Note.G4,8, Note.REST,4, Note.GS4,4,//23
              Note.A4,8, Note.F5,4, Note.F5,8, Note.A4,2,
              Note.D5,-8, Note.A5,-8, Note.A5,-8, Note.A5,-8, Note.G5,-8, Note.F5,-8,

              Note.E5,8, Note.C5,4, Note.A4,8, Note.G4,2, //26
              Note.E5,8, Note.C5,4, Note.G4,8, Note.REST,4, Note.GS4,4,
              Note.A4,8, Note.F5,4, Note.F5,8, Note.A4,2,
              Note.B4,8, Note.F5,4, Note.F5,8, Note.F5,-8, Note.E5,-8, Note.D5,-8,
              Note.C5,8, Note.E4,4, Note.E4,8, Note.C4,2,

              Note.E5,8, Note.C5,4, Note.G4,8, Note.REST,4, Note.GS4,4,//repeats from 23
              Note.A4,8, Note.F5,4, Note.F5,8, Note.A4,2,
              Note.D5,-8, Note.A5,-8, Note.A5,-8, Note.A5,-8, Note.G5,-8, Note.F5,-8,

              Note.E5,8, Note.C5,4, Note.A4,8, Note.G4,2, //26
              Note.E5,8, Note.C5,4, Note.G4,8, Note.REST,4, Note.GS4,4,
              Note.A4,8, Note.F5,4, Note.F5,8, Note.A4,2,
              Note.B4,8, Note.F5,4, Note.F5,8, Note.F5,-8, Note.E5,-8, Note.D5,-8,
              Note.C5,8, Note.E4,4, Note.E4,8, Note.C4,2,
              Note.C5,8, Note.C5,4, Note.C5,8, Note.REST,8, Note.C5,8, Note.D5,8, Note.E5,8,
              Note.REST,1,

              Note.C5,8, Note.C5,4, Note.C5,8, Note.REST,8, Note.C5,8, Note.D5,4, //33
              Note.E5,8, Note.C5,4, Note.A4,8, Note.G4,2,
              Note.E5,8, Note.E5,8, Note.REST,8, Note.E5,8, Note.REST,8, Note.C5,8, Note.E5,4,
              Note.G5,4, Note.REST,4, Note.G4,4, Note.REST,4,
              Note.E5,8, Note.C5,4, Note.G4,8, Note.REST,4, Note.GS4,4,
              Note.A4,8, Note.F5,4, Note.F5,8, Note.A4,2,
              Note.D5,-8, Note.A5,-8, Note.A5,-8, Note.A5,-8, Note.G5,-8, Note.F5,-8,

              Note.E5,8, Note.C5,4, Note.A4,8, Note.G4,2, //40
              Note.E5,8, Note.C5,4, Note.G4,8, Note.REST,4, Note.GS4,4,
              Note.A4,8, Note.F5,4, Note.F5,8, Note.A4,2,
              Note.B4,8, Note.F5,4, Note.F5,8, Note.F5,-8, Note.E5,-8, Note.D5,-8,
              Note.C5,8, Note.E4,4, Note.E4,8, Note.C4,2,
  
              //game over sound
              Note.C5,-4, Note.G4,-4, Note.E4,4, //45
              Note.A4,-8, Note.B4,-8, Note.A4,-8, Note.GS4,-8, Note.AS4,-8, Note.GS4,-8,
              Note.G4,8, Note.D4,8, Note.E4,-2
            };

            PlayMusicInternal(melody, tempo);
        }

        public void PlayNokiaRingtone()
        {
            // change this to make the song slower or faster
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

        public void PlayPiratesOfTheCaribbean()
        {
            // change this to make the song slower or faster
            int tempo = 140;

            // notes of the moledy followed by the duration.
            // a 4 means a quarter note, 8 an eighteenth , 16 sixteenth, so on
            // !!negative numbers are used to represent dotted notes,
            // so -4 means a dotted quarter note, that is, a quarter plus an eighteenth!!
            int[] melody =
            {

              // Pirates of the Caribbean theme
              // Score available at https://musescore.com/user/127542/scores/123606
              // Theme by Klaus Badelt
              // Tab by bassmasta.net, removed some repetition.
  
              Note.D4, 4, Note.F4, 4, Note.A4, 4, Note.A4, 4, Note.A4, 4, Note.G4, 2,
              Note.A4, 4, Note.C5, 4, Note.C5, 4, Note.C5, 4, Note.B4, 4, Note.A4, 2,
              Note.D4, 4, Note.F4, 4, Note.A4, 4, Note.A4, 4, Note.A4, 4, Note.G4, 2,
              Note.A4, 4, Note.C5, 4, Note.C5, 4, Note.C5, 4, Note.B4, 4, Note.A4, 2,
              Note.A4, 4, Note.A5, 4, Note.A4, 4, Note.G4, 4, Note.F4, 8, Note.E4, 8, Note.F4, 2,
              Note.A4, 4, Note.A5, 4, Note.A4, 4, Note.G4, 4, Note.F4, 8, Note.G4, 8, Note.A4, 2,
              Note.D4, 4, Note.F4, 4, Note.A4, 4, Note.A4, 4, Note.A4, 4, Note.G4, 2,
              Note.A4, 4, Note.C5, 4, Note.C5, 4, Note.C5
            };

            PlayMusicInternal(melody, tempo);
        }

        public void PlayPinkPanther()
        {
            // change this to make the song slower or faster
            int tempo = 120;
            // notes of the moledy followed by the duration.
            // a 4 means a quarter note, 8 an eighteenth , 16 sixteenth, so on
            // !!negative numbers are used to represent dotted notes,
            // so -4 means a dotted quarter note, that is, a quarter plus an eighteenth!!
            int[] melody = {
              // Pink Panther theme
              // Score available at https://musescore.com/benedictsong/the-pink-panther
              // Theme by Masato Nakamura, arranged by Teddy Mason

              Note.REST,2, Note.REST,4, Note.REST,8, Note.DS4,8,
              Note.E4,-4, Note.REST,8, Note.FS4,8, Note.G4,-4, Note.REST,8, Note.DS4,8,
              Note.E4,-8, Note.FS4,8,  Note.G4,-8, Note.C5,8, Note.B4,-8, Note.E4,8, Note.G4,-8, Note.B4,8,
              Note.AS4,2, Note.A4,-16, Note.G4,-16, Note.E4,-16, Note.D4,-16,
              Note.E4,2, Note.REST,4, Note.REST,8, Note.DS4,4,

              Note.E4,-4, Note.REST,8, Note.FS4,8, Note.G4,-4, Note.REST,8, Note.DS4,8,
              Note.E4,-8, Note.FS4,8,  Note.G4,-8, Note.C5,8, Note.B4,-8, Note.G4,8, Note.B4,-8, Note.E5,8,
              Note.DS5,1,
              Note.D5,2, Note.REST,4, Note.REST,8, Note.DS4,8,
              Note.E4,-4, Note.REST,8, Note.FS4,8, Note.G4,-4, Note.REST,8, Note.DS4,8,
              Note.E4,-8, Note.FS4,8,  Note.G4,-8, Note.C5,8, Note.B4,-8, Note.E4,8, Note.G4,-8, Note.B4,8,

              Note.AS4,2, Note.A4,-16, Note.G4,-16, Note.E4,-16, Note.D4,-16,
              Note.E4,-4, Note.REST,4,
              Note.REST,4, Note.E5,-8, Note.D5,8, Note.B4,-8, Note.A4,8, Note.G4,-8, Note.E4,-8,
              Note.AS4,16, Note.A4,-8, Note.AS4,16, Note.A4,-8, Note.AS4,16, Note.A4,-8, Note.AS4,16, Note.A4,-8,
              Note.G4,-16, Note.E4,-16, Note.D4,-16, Note.E4,16, Note.E4,16, Note.E4,2,

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
    }
}