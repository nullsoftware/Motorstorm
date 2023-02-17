using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NullSoftware.Serialization;
using UnityEngine;

public class GameData
{
    #region Fields

    private static readonly BinarySerializer<GameData> _serializer 
        = new BinarySerializer<GameData>();

    #endregion

    #region Data properties

    public string Name { get; set; }
    public float Score { get; set; }
    public uint Coins { get; set; }

    #endregion

    #region Methods

    public static bool TryLoad(string filename, out GameData gameData)
    {
        if (!File.Exists(filename))
        {
            gameData = null;
            return false;
        }

        using (FileStream fs = new FileStream(filename, FileMode.Open))
        {
            gameData = _serializer.Deserialize(fs);
            return true;
        }
    }

    public static void Save(string filename, GameData data)
    {
        using (FileStream fs = new FileStream(filename, FileMode.Create))
        {
            _serializer.Serialize(fs, data);
        }
    }

    #endregion
}
