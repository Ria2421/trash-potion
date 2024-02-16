//
// プレイヤー情報スクリプト
// Name:西浦晃太 Date:2/8
//

public class Player
{
    public int PlayerNo;
    //クリアしたか
    public bool IsClear;

    //相手の赤い駒をとるとダメージ
    public int Hp = 4;

    //相手の青い駒をとると加点
    public int Score;

    public Player(int playerno)
    {
        this.PlayerNo = playerno;
    }

    //public string GetPlayerName()
    //{
    //    string ret = "";
    //    string playerName = PlayerNo + "P";
    //    ret = playerName;

    //    return ret;
    //}
}
