//
// プレイヤー情報スクリプト
// Name:西浦晃太 Date:2/8
//

public class Player4
{
    public bool isPlayer;
    public int PlayerNo;
    //クリアしたか
    public bool IsClear;

    //相手の赤い駒をとるとダメージ
    public int Hp = 1;

    //相手の青い駒をとると加点
    public int Score;

    public Player4(bool isPlayer, int playerno)
    {
        this.isPlayer = isPlayer;
        this.PlayerNo = playerno;
    }

    public string GetPlayerName()
    {
        string ret = "";
        string playerName = PlayerNo + "P";

        if (!isPlayer)
        {
            playerName = "名無しの博士";
        }
        else
        {
            ret = playerName;
        }

        return ret;
    }
}
