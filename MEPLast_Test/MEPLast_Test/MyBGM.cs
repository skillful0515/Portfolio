using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FK_CLI;
using System.Threading;

namespace MEPLast_Test
{
    // BGM用クラス
    class MyBGM : IDisposable
    {
        public bool EndStatus { get; set; } // 終了指示用用プロパティ
        private fk_AudioStream bgm;
        private bool openStatus;

        // コンストラクタ 引数は音源ファイル名（Ogg形式）
        public MyBGM(string argFileName)
        {
            EndStatus = false;
            bgm = new fk_AudioStream();
            openStatus = bgm.Open(argFileName);
            if (openStatus == false)
            {
                Console.WriteLine("Audio File Error");
            }
        }

        // BGM再生処理
        public void Start()
        {
            if (openStatus == false) return;
            bgm.LoopMode = true;
            bgm.Gain = 0.5;
            while (EndStatus == false)
            {
                bgm.Play();
                Thread.Sleep(50);
            }
        }

        // 音量用プロパティ
        public double Gain
        {
            set
            {
                bgm.Gain = value;
            }
        }

        // スレッド終了時処理
        public void Dispose()
        {
            bgm.Dispose();
        }
    }

    // SEクラス
    class MySE : IDisposable
    {
        public bool EndStatus { get; set; } // 終了指示用プロパティ
        private fk_AudioWavBuffer[] se;
        private bool[] openStatus;
        private bool[] playStatus;

        public MySE(int argNum)
        {
            EndStatus = false;
            if (argNum < 1) return;
            se = new fk_AudioWavBuffer[argNum];
            openStatus = new bool[argNum];
            playStatus = new bool[argNum];

            for (int i = 0; i < argNum; i++)
            {
                se[i] = new fk_AudioWavBuffer();
                openStatus[i] = false;
                playStatus[i] = false;
            }
        }

        // SE音源読み込みメソッド（WAV形式）
        public bool LoadData(int argID, string argFileName)
        {
            if (argID < 0 || argID >= se.Length)
            {
                return false;
            }

            openStatus[argID] = se[argID].Open(argFileName);
            if (openStatus[argID] == false)
            {
                Console.WriteLine("Audio File ({0}) Open Error.", argFileName);
            }
            se[argID].LoopMode = false;
            se[argID].Gain = 0.5;
            return true;
        }

        // SE開始メソッド
        public void StartSE(int argID)
        {
            if (argID < 0 || argID >= se.Length) return;
            playStatus[argID] = true;
            se[argID].Seek(0.0);
        }

        // SE再生処理
        public void Start()
        {
            int i;

            for (i = 0; i < se.Length; i++)
            {
                if (openStatus[i] == false) return;
            }

            while (EndStatus == false)
            {
                for (i = 0; i < se.Length; i++)
                {
                    if (playStatus[i] == true)
                    {
                        playStatus[i] = se[i].Play();
                    }
                }
                Thread.Sleep(10);
            }
        }

        // 効果音音量設定
        public void SetGain(int _id, double value)
        {
            se[_id].Gain = value;
        }

        // スレッド終了処理
        public void Dispose()
        {
            for (int i = 0; i < se.Length; i++)
            {
                se[i].Dispose();
            }
        }
    }
}
