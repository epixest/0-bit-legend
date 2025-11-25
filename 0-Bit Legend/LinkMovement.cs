using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using ZeldaEmulator;

namespace ZeldaEmulator;

public class LinkMovement : MainProgram
{
    private int posX;
    private int posY;

    private int preHitPosX;
    private int preHitPosY;

    private string[] storage_map = new string[20] { " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " };
    private static string[] storage_sword = new string[6];
    private static string[] storage_detect_enemy = new string[6];

    private string prev = "w";
    private string prev2 = "a";

    private bool debounce = false;
    private bool spawnRupee = false;

    public int GetPosX()
    {
        return posX;
    }
    public int GetPosY()
    {
        return posY;
    }
    public string GetPrev()
    {
        return prev;
    }
    public string GetPrev2()
    {
        return prev2;
    }
    public void SetPosX(int posX)
    {
        this.posX = posX;
    }
    public void SetPosY(int posY)
    {
        this.posY = posY;
    }
    public void SetPreHitPosX(int posX)
    {
        this.preHitPosX = posX;
    }
    public void SetPreHitPosY(int posY)
    {
        this.preHitPosY = posY;
    }
    public void SetPrev(string prev)
    {
        this.prev = prev;
    }
    public void SetSpawnRupee(bool spawnRupee)
    {
        this.spawnRupee = spawnRupee;
    }

    public void Attack(string prev, bool attacking)
    {
        if (prev == "w" && posY - 3 > 0)
        {
            if (attacking == false)
            {
                storage_sword[0] = map[posX - 1, posY - 2];
                storage_sword[1] = map[posX, posY - 2];
                storage_sword[2] = map[posX + 1, posY - 2];
                storage_sword[3] = map[posX, posY - 3];
                storage_sword[4] = map[posX, posY - 4];

                map[posX - 1, posY - 2] = "-";
                map[posX, posY - 2] = "-";
                map[posX + 1, posY - 2] = "-";
                map[posX, posY - 3] = "S";
                map[posX, posY - 4] = "S";

                preHitPosX = posX;
                preHitPosY = posY;
            }
            else
            {
                storage_detect_enemy[0] = map[preHitPosX - 1, preHitPosY - 2];
                storage_detect_enemy[1] = map[preHitPosX, preHitPosY - 2];
                storage_detect_enemy[2] = map[preHitPosX + 1, preHitPosY - 2];
                storage_detect_enemy[3] = map[preHitPosX, preHitPosY - 3];
                storage_detect_enemy[4] = map[preHitPosX, preHitPosY - 4];

                int[,] swordArr = new int[2, 5];
                swordArr[0, 0] = preHitPosX - 1;
                swordArr[1, 0] = preHitPosY - 2;
                swordArr[0, 1] = preHitPosX;
                swordArr[1, 1] = preHitPosY - 2;
                swordArr[0, 2] = preHitPosX + 1;
                swordArr[1, 2] = preHitPosY - 2;
                swordArr[0, 3] = preHitPosX;
                swordArr[1, 3] = preHitPosY - 3;
                swordArr[0, 4] = preHitPosX;
                swordArr[1, 4] = preHitPosY - 4;

                Stab(swordArr, prev, 5, 1);
            }

            UpdateRow(preHitPosY - 2);
            UpdateRow(preHitPosY - 3);
            UpdateRow(preHitPosY - 4);
        }
        else if (prev == "a" && posX - 4 > 0)
        {
            if (attacking == false)
            {
                storage_sword[0] = map[posX - 3, posY];
                storage_sword[1] = map[posX - 3, posY + 1];
                storage_sword[2] = map[posX - 3, posY + 2];
                storage_sword[3] = map[posX - 4, posY + 1];
                storage_sword[4] = map[posX - 5, posY + 1];
                storage_sword[5] = map[posX - 6, posY + 1];

                map[posX - 3, posY] = "-";
                map[posX - 3, posY + 1] = "-";
                map[posX - 3, posY + 2] = "-";
                map[posX - 4, posY + 1] = "S";
                map[posX - 5, posY + 1] = "S";
                map[posX - 6, posY + 1] = "S";

                preHitPosX = posX;
                preHitPosY = posY;
            }
            else
            {
                storage_detect_enemy[0] = map[preHitPosX - 3, preHitPosY];
                storage_detect_enemy[1] = map[preHitPosX - 3, preHitPosY + 1];
                storage_detect_enemy[2] = map[preHitPosX - 3, preHitPosY + 2];
                storage_detect_enemy[3] = map[preHitPosX - 4, preHitPosY + 1];
                storage_detect_enemy[4] = map[preHitPosX - 5, preHitPosY + 1];
                storage_detect_enemy[5] = map[preHitPosX - 6, preHitPosY + 1];

                int[,] swordArr = new int[2, 6];
                swordArr[0, 0] = preHitPosX - 3;
                swordArr[1, 0] = preHitPosY;
                swordArr[0, 1] = preHitPosX - 3;
                swordArr[1, 1] = preHitPosY + 1;
                swordArr[0, 2] = preHitPosX - 3;
                swordArr[1, 2] = preHitPosY + 2;
                swordArr[0, 3] = preHitPosX - 4;
                swordArr[1, 3] = preHitPosY + 1;
                swordArr[0, 4] = preHitPosX - 5;
                swordArr[1, 4] = preHitPosY + 1;
                swordArr[0, 5] = preHitPosX - 6;
                swordArr[1, 5] = preHitPosY + 1;

                Stab(swordArr, prev, 6, 1);
            }

            UpdateRow(preHitPosY);
            UpdateRow(preHitPosY + 1);
            UpdateRow(preHitPosY + 2);
        }
        else if (prev == "s" && posY + 4 < 33)
        {
            if (attacking == false)
            {
                storage_sword[0] = map[posX - 1, posY + 3];
                storage_sword[1] = map[posX, posY + 3];
                storage_sword[2] = map[posX + 1, posY + 3];
                storage_sword[3] = map[posX, posY + 4];
                storage_sword[4] = map[posX, posY + 5];

                map[posX - 1, posY + 3] = "-";
                map[posX, posY + 3] = "-";
                map[posX + 1, posY + 3] = "-";
                map[posX, posY + 4] = "S";
                map[posX, posY + 5] = "S";

                preHitPosX = posX;
                preHitPosY = posY;
            }
            else
            {
                storage_detect_enemy[0] = map[preHitPosX - 1, preHitPosY + 3];
                storage_detect_enemy[1] = map[preHitPosX, preHitPosY + 3];
                storage_detect_enemy[2] = map[preHitPosX + 1, preHitPosY + 3];
                storage_detect_enemy[3] = map[preHitPosX, preHitPosY + 4];
                storage_detect_enemy[4] = map[preHitPosX, preHitPosY + 5];

                int[,] swordArr = new int[2, 5];
                swordArr[0, 0] = preHitPosX - 1;
                swordArr[1, 0] = preHitPosY + 3;
                swordArr[0, 1] = preHitPosX;
                swordArr[1, 1] = preHitPosY + 3;
                swordArr[0, 2] = preHitPosX + 1;
                swordArr[1, 2] = preHitPosY + 3;
                swordArr[0, 3] = preHitPosX;
                swordArr[1, 3] = preHitPosY + 4;
                swordArr[0, 4] = preHitPosX;
                swordArr[1, 4] = preHitPosY + 5;

                Stab(swordArr, prev, 5, 1);
            }

            UpdateRow(preHitPosY + 3);
            UpdateRow(preHitPosY + 4);
            UpdateRow(preHitPosY + 5);
        }
        else if (prev == "d" && posX + 6 < 102)
        {
            if (attacking == false)
            {
                storage_sword[0] = map[posX + 3, posY];
                storage_sword[1] = map[posX + 3, posY + 1];
                storage_sword[2] = map[posX + 3, posY + 2];
                storage_sword[3] = map[posX + 4, posY + 1];
                storage_sword[4] = map[posX + 5, posY + 1];
                storage_sword[5] = map[posX + 6, posY + 1];

                map[posX + 3, posY] = "-";
                map[posX + 3, posY + 1] = "-";
                map[posX + 3, posY + 2] = "-";
                map[posX + 4, posY + 1] = "S";
                map[posX + 5, posY + 1] = "S";
                map[posX + 6, posY + 1] = "S";

                preHitPosX = posX;
                preHitPosY = posY;
            }
            else
            {
                storage_detect_enemy[0] = map[preHitPosX + 3, preHitPosY];
                storage_detect_enemy[1] = map[preHitPosX + 3, preHitPosY + 1];
                storage_detect_enemy[2] = map[preHitPosX + 3, preHitPosY + 2];
                storage_detect_enemy[3] = map[preHitPosX + 4, preHitPosY + 1];
                storage_detect_enemy[4] = map[preHitPosX + 5, preHitPosY + 1];
                storage_detect_enemy[5] = map[preHitPosX + 6, preHitPosY + 1];

                int[,] swordArr = new int[2, 6];
                swordArr[0, 0] = preHitPosX + 3;
                swordArr[1, 0] = preHitPosY;
                swordArr[0, 1] = preHitPosX + 3;
                swordArr[1, 1] = preHitPosY + 1;
                swordArr[0, 2] = preHitPosX + 3;
                swordArr[1, 2] = preHitPosY + 2;
                swordArr[0, 3] = preHitPosX + 4;
                swordArr[1, 3] = preHitPosY + 1;
                swordArr[0, 4] = preHitPosX + 5;
                swordArr[1, 4] = preHitPosY + 1;
                swordArr[0, 5] = preHitPosX + 6;
                swordArr[1, 5] = preHitPosY + 1;

                Stab(swordArr, prev, 6, 1);
            }

            UpdateRow(preHitPosY);
            UpdateRow(preHitPosY + 1);
            UpdateRow(preHitPosY + 2);
        }
    }

    public void Stab(int[,] swordArr, string prev, int amt, int dmg)
    {
        bool hit = false;
        for (int i = 0; i < amt; i++)
        {
            if (storage_sword[i] == "t" || storage_sword[i] == "n" || storage_sword[i] == "B" || storage_sword[i] == "{" || storage_sword[i] == "}" || storage_sword[i] == "F" || storage_detect_enemy[i] == "t" || storage_detect_enemy[i] == "n" || storage_detect_enemy[i] == "B" || storage_detect_enemy[i] == "{" || storage_detect_enemy[i] == "}" || storage_detect_enemy[i] == "F")
            {
                hit = true;
                if (enemyMovement.TakeDamage(swordArr[0, i], swordArr[1, i], prev, dmg) && spawnRupee)
                {
                    spawnRupee = false;
                    enemyMovement.SpawnRupee();
                }
                break;
            }
        }
        if (!hit)
        {
            StoreSword(prev);
        }
    }

    public void StoreSword(string prev)
    {
        for (int i = 0; i < 6; i++)
        {
            if (storage_sword[i] == "t" || storage_sword[i] == "^" || storage_sword[i] == "n" || storage_sword[i] == "0" || storage_sword[i] == "B" || storage_sword[i] == "{" || storage_sword[i] == "}" || storage_sword[i] == "F" || storage_sword[i] == "S" || storage_sword[i] == ">" || storage_sword[i] == "*")
            {
                storage_sword[i] = " ";
            }
        }

        if (prev == "w")
        {
            map[preHitPosX - 1, preHitPosY - 2] = storage_sword[0];
            map[preHitPosX, preHitPosY - 2] = storage_sword[1];
            map[preHitPosX + 1, preHitPosY - 2] = storage_sword[2];
            map[preHitPosX, preHitPosY - 3] = storage_sword[3];
            map[preHitPosX, preHitPosY - 4] = storage_sword[4];
        }
        else if (prev == "a")
        {
            map[preHitPosX - 3, preHitPosY] = storage_sword[0];
            map[preHitPosX - 3, preHitPosY + 1] = storage_sword[1];
            map[preHitPosX - 3, preHitPosY + 2] = storage_sword[2];
            map[preHitPosX - 4, preHitPosY + 1] = storage_sword[3];
            map[preHitPosX - 5, preHitPosY + 1] = storage_sword[4];
            map[preHitPosX - 6, preHitPosY + 1] = storage_sword[5];
        }
        else if (prev == "s")
        {
            map[preHitPosX - 1, preHitPosY + 3] = storage_sword[0];
            map[preHitPosX, preHitPosY + 3] = storage_sword[1];
            map[preHitPosX + 1, preHitPosY + 3] = storage_sword[2];
            map[preHitPosX, preHitPosY + 4] = storage_sword[3];
            map[preHitPosX, preHitPosY + 5] = storage_sword[4];
        }
        else if (prev == "d")
        {
            map[preHitPosX + 3, preHitPosY] = storage_sword[0];
            map[preHitPosX + 3, preHitPosY + 1] = storage_sword[1];
            map[preHitPosX + 3, preHitPosY + 2] = storage_sword[2];
            map[preHitPosX + 4, preHitPosY + 1] = storage_sword[3];
            map[preHitPosX + 5, preHitPosY + 1] = storage_sword[4];
            map[preHitPosX + 6, preHitPosY + 1] = storage_sword[5];
        }
    }

    public void MoveLink(int posX, int posY, string direction, bool spawn)
    {
        if (spawn)
        {
            this.posX = posX;
            this.posY = posY;

            storage_map[0] = map[posX - 2, posY - 1];
            storage_map[1] = map[posX - 1, posY - 1];
            storage_map[2] = map[posX, posY - 1];
            storage_map[3] = map[posX + 1, posY - 1];
            storage_map[4] = map[posX + 2, posY - 1];

            storage_map[5] = map[posX - 2, posY];
            storage_map[6] = map[posX - 1, posY];
            storage_map[7] = map[posX, posY];
            storage_map[8] = map[posX + 1, posY];
            storage_map[9] = map[posX + 2, posY];

            storage_map[10] = map[posX - 2, posY + 1];
            storage_map[11] = map[posX - 1, posY + 1];
            storage_map[12] = map[posX, posY + 1];
            storage_map[13] = map[posX + 1, posY + 1];
            storage_map[14] = map[posX + 2, posY + 1];

            storage_map[15] = map[posX - 2, posY + 2];
            storage_map[16] = map[posX - 1, posY + 2];
            storage_map[17] = map[posX, posY + 2];
            storage_map[18] = map[posX + 1, posY + 2];
            storage_map[19] = map[posX + 2, posY + 2];
        }

        prev = direction;

        if (direction == "w")
        {
            if (this.posX == 21 && ((currentMap == 4 && posY > 9) || currentMap == 2))
            {
                if (posY > 1)
                {
                    for (int y = this.posY - 2; y <= this.posY + 3; y++)
                    {
                        for (int x = this.posX - 3; x <= this.posX + 3; x++)
                        {
                            map[x, y] = "~";
                        }
                    }

                    this.posY -= 1;
                    DeployRaft(prev2);

                    UpdateRow(this.posY + 4);
                }
                else
                {
                    LoadMap(4, 21, 29, "w");
                }
            }
            else if (posY >= 1 && !(this.posX == 21 && (currentMap == 4 || currentMap == 2)))
            {
                IsTouching(posX, posY, "r");
                StoreChar(this.posX, this.posY);
                bool inCave = false;

                if (currentMap == 6 && (IsTouching(posX, posY, "-") || IsTouching(posX, posY, "S")))
                {
                    hasSword = true;
                    LoadMap(6, posX, posY, direction);
                }
                else if (currentMap == 7 && IsTouching(posX, posY, "*") && rupees >= 35)
                {
                    rupees -= 35;

                    hasRaft = true;
                    LoadMap(7, posX, posY, direction);
                }
                else if (currentMap == 7 && IsTouching(posX, posY, "Y") && rupees >= 5)
                {
                    rupees -= 10;
                    keys++;
                }
                else if (currentMap == 7 && IsTouching(posX, posY, "#") && rupees >= 15)
                {
                    rupees -= 25;

                    hasArmor = true;
                    LoadMap(7, posX, posY, direction);
                }
                else if (currentMap == 9 && IsTouching(posX, posY, "X") && !IsTouching(posX, posY, "=") && cDoor1 && cDoor2 && cDoor3 && cEnemies1 <= 0 && cEnemies2 <= 0 && !debounce)
                {
                    LoadMap(12, 50, 24, direction);
                }
                else if (currentMap == 9 && this.posX >= 48 && this.posX <= 52 && this.posY == 7 && !cDoor3 && keys > 0)
                {
                    debounce = true;
                    keys--;

                    cDoor3 = true;
                    LoadMap(9, this.posX, this.posY, direction);
                }

                if (!IsTouching(posX, posY, "=") && !IsTouching(posX, posY, "X") && !IsTouching(posX, posY, "t") && !IsTouching(posX, posY, "n") && !IsTouching(posX, posY, "B") && !IsTouching(posX, posY, "{") && !IsTouching(posX, posY, "}") && !IsTouching(posX, posY, "S") && !IsTouching(posX, posY, "<") && !IsTouching(posX, posY, ">") && !IsTouching(posX, posY, "*") && !(IsTouching(posX, posY, "F") && currentMap != 7) && !IsTouching(posX, posY, "~") && !((currentMap == 6 || currentMap == 7) && posY < 17) && ((currentMap >= 9 && !IsTouching(posX, posY, "/")) || currentMap < 9))
                {
                    if (IsTouching(posX, posY, "/"))
                    {
                        inCave = true;
                    }

                    StoreChar(posX, posY);
                    BuildChar(posX, posY, direction);

                    UpdateRow(posY - 1);
                    UpdateRow(posY);
                    UpdateRow(posY + 1);
                    UpdateRow(posY + 2);

                    this.posX = posX;
                    this.posY = posY;
                }
                else if (IsTouching(posX, posY, "t") || IsTouching(posX, posY, "n") || IsTouching(posX, posY, "B") || IsTouching(posX, posY, "{") || IsTouching(posX, posY, "}") || IsTouching(posX, posY, "S") || IsTouching(posX, posY, "<") || (IsTouching(posX, posY, "F") && currentMap != 7))
                {
                    BuildChar(this.posX, this.posY, direction);

                    UpdateRow(this.posY - 1);
                    UpdateRow(this.posY);
                    UpdateRow(this.posY + 1);
                    UpdateRow(this.posY + 2);

                    Hit();
                }
                else if (currentMap == 13 && IsTouching(posX, posY, "~"))
                {
                    BuildChar(this.posX, this.posY, direction);

                    UpdateRow(this.posY - 1);
                    UpdateRow(this.posY);
                    UpdateRow(this.posY + 1);
                    UpdateRow(this.posY + 2);

                    LoadMap(13, 58, 15, "a");
                    gameOver = true;
                }
                else
                {
                    BuildChar(this.posX, this.posY, direction);

                    UpdateRow(this.posY - 1);
                    UpdateRow(this.posY);
                    UpdateRow(this.posY + 1);
                    UpdateRow(this.posY + 2);
                }

                if (inCave)
                {
                    Wait(2);
                }
            }
            else
            {
                if (currentMap == 0)
                {
                    LoadMap(1, 63, 29, "w");
                }
                else if (currentMap == 2)
                {
                    if (posX > 29)
                    {
                        LoadMap(4, 55, 30, "w");
                    }
                    else if (posX == 21)
                    {
                        LoadMap(4, 21, 29, "w");
                    }
                    else
                    {
                        LoadMap(4, 10, 29, "w");
                    }
                }
                else if (currentMap == 3)
                {
                    LoadMap(5, 49, 30, "w");
                }
            }
            if (cText)
            {
                cText = false;
                LoadMap(9, posX, posY, "w");
            }
        }
        else if (direction == "a")
        {
            prev2 = "a";
            if (posX >= 2)
            {
                IsTouching(posX, posY, "r");
                StoreChar(this.posX, this.posY);

                if (currentMap == 6 && (IsTouching(posX, posY, "-") || IsTouching(posX, posY, "S")))
                {
                    hasSword = true;
                    LoadMap(6, posX, posY, direction);
                }
                else if (currentMap == 7 && IsTouching(posX, posY, "*") && rupees >= 35)
                {
                    rupees -= 35;

                    hasRaft = true;
                    LoadMap(7, posX, posY, direction);
                }
                else if (currentMap == 7 && IsTouching(posX, posY, "Y") && rupees >= 10)
                {
                    rupees -= 10;
                    keys++;
                }
                else if (currentMap == 7 && IsTouching(posX, posY, "#") && rupees >= 25)
                {
                    rupees -= 25;

                    hasArmor = true;
                    LoadMap(7, posX, posY, direction);
                }
                else if (currentMap == 9 && IsTouching(posX, posY, "X") && !IsTouching(posX, posY, "=") && cDoor1 && !debounce)
                {
                    LoadMap(10, 87, 15, direction);
                }
                else if (currentMap == 9 && this.posX == 14 && this.posY >= 14 && this.posY <= 16 && !cDoor1 && keys > 0)
                {
                    debounce = true;
                    keys--;

                    cDoor1 = true;
                    LoadMap(9, this.posX, this.posY, direction);
                }

                if (!IsTouching(posX, posY, "=") && !IsTouching(posX, posY, "X") && !IsTouching(posX, posY, "t") && !IsTouching(posX, posY, "n") && !IsTouching(posX, posY, "B") && !IsTouching(posX, posY, "{") && !IsTouching(posX, posY, "}") && !IsTouching(posX, posY, "S") && !IsTouching(posX, posY, "<") && !(IsTouching(posX, posY, "F") && currentMap != 7) && !IsTouching(posX, posY, "~") && ((currentMap >= 9 && !IsTouching(posX, posY, "/")) || currentMap < 9))
                {
                    StoreChar(posX, posY);
                    BuildChar(posX, posY, direction);

                    UpdateRow(posY - 1);
                    UpdateRow(posY);
                    UpdateRow(posY + 1);
                    UpdateRow(posY + 2);

                    this.posX = posX;
                    this.posY = posY;
                }
                else if (IsTouching(posX, posY, "t") || IsTouching(posX, posY, "n") || IsTouching(posX, posY, "B") || IsTouching(posX, posY, "{") || IsTouching(posX, posY, "}") || IsTouching(posX, posY, "S") || IsTouching(posX, posY, "<") || (IsTouching(posX, posY, "F") && currentMap != 7))
                {
                    BuildChar(this.posX, this.posY, direction);

                    UpdateRow(this.posY - 1);
                    UpdateRow(this.posY);
                    UpdateRow(this.posY + 1);
                    UpdateRow(this.posY + 2);

                    Hit();
                }
                else if (IsTouching(posX, posY, "~") && this.posX != 21 && hasRaft && !IsTouching(posX, posY, "=") && !IsTouching(posX, posY, "X") && !IsTouching(posX, posY, "t") && !IsTouching(posX, posY, "n"))
                {
                    StoreChar(this.posX, this.posY);

                    UpdateRow(this.posY - 1);
                    UpdateRow(this.posY);
                    UpdateRow(this.posY + 1);
                    UpdateRow(this.posY + 2);

                    this.posX = 21;
                    DeployRaft("a");
                    wait = 150;
                }
                else if (this.posX == 21 && ((posY > 11 && currentMap == 4) || currentMap == 2) && ((currentMap == 2 && posY < 25) || currentMap == 4))
                {
                    BuildChar(this.posX, this.posY, direction);

                    UpdateRow(this.posY - 1);
                    UpdateRow(this.posY);
                    UpdateRow(this.posY + 1);
                    UpdateRow(this.posY + 2);

                    for (int y = this.posY - 2; y <= this.posY + 3; y++)
                    {
                        for (int x = this.posX - 3; x <= this.posX + 3; x++)
                        {
                            map[x, y] = "~";
                        }
                    }

                    this.posX = 11;
                    posX = 11;

                    BuildChar(posX, posY, direction);

                    UpdateRow(posY - 2);
                    UpdateRow(posY - 1);
                    UpdateRow(posY);
                    UpdateRow(posY + 1);
                    UpdateRow(posY + 2);
                    UpdateRow(posY + 3);
                }
                else if (currentMap == 11 && IsTouching(posX, posY, "X") && !IsTouching(posX, posY, "="))
                {
                    LoadMap(9, 86, 15, "a");
                }
                else if (currentMap == 13 && IsTouching(posX, posY, "X") && !IsTouching(posX, posY, "="))
                {
                    LoadMap(12, 86, 15, "a");
                }
                else
                {
                    BuildChar(this.posX, this.posY, direction);

                    UpdateRow(this.posY - 1);
                    UpdateRow(this.posY);
                    UpdateRow(this.posY + 1);
                    UpdateRow(this.posY + 2);
                }

            }
            else
            {
                if (currentMap == 0)
                {
                    LoadMap(2, 99, 12, "a");
                }
                else if (currentMap == 3)
                {
                    LoadMap(0, 98, 17, "a");
                }
                else if (currentMap == 1)
                {
                    LoadMap(4, 98, 13, "a");
                }
                else if (currentMap == 5)
                {
                    LoadMap(1, 98, 16, "a");
                }
                else if (currentMap == 4)
                {
                    LoadMap(8, 99, 16, "a");
                }
            }
            if (cText)
            {
                cText = false;
                LoadMap(9, posX, posY, "w");
            }
        }
        else if (direction == "s")
        {
            if ((currentMap == 2 || currentMap == 4) && posX == 21)
            {
                if ((posY < 30) && ((currentMap == 2 && posY < 27) || currentMap == 4))
                {
                    for (int y = this.posY - 2; y <= this.posY + 3; y++)
                    {
                        for (int x = this.posX - 3; x <= this.posX + 3; x++)
                        {
                            map[x, y] = "~";
                        }
                    }

                    this.posY += 1;
                    DeployRaft(prev2);

                    UpdateRow(this.posY - 3);
                }
                else if (currentMap == 4)
                {
                    LoadMap(2, 21, 2, "s");
                }
            }
            else if (posY <= 29)
            {
                IsTouching(posX, posY, "r");

                StoreChar(this.posX, this.posY);
                if (!IsTouching(posX, posY, "=") && !IsTouching(posX, posY, "X") && !IsTouching(posX, posY, "t") && !IsTouching(posX, posY, "n") && !IsTouching(posX, posY, "B") && !IsTouching(posX, posY, "{") && !IsTouching(posX, posY, "}") && !IsTouching(posX, posY, "S") && !IsTouching(posX, posY, "<") && !(IsTouching(posX, posY, "F") && currentMap != 7) && !IsTouching(posX, posY, "~") && ((currentMap >= 9 && !IsTouching(posX, posY, "/")) || currentMap < 9))
                {
                    StoreChar(posX, posY);
                    BuildChar(posX, posY, direction);

                    UpdateRow(posY - 1);
                    UpdateRow(posY);
                    UpdateRow(posY + 1);
                    UpdateRow(posY + 2);

                    this.posX = posX;
                    this.posY = posY;
                }
                else if (IsTouching(posX, posY, "t") || IsTouching(posX, posY, "n") || IsTouching(posX, posY, "B") || IsTouching(posX, posY, "{") || IsTouching(posX, posY, "}") || IsTouching(posX, posY, "S") || IsTouching(posX, posY, "<") || (IsTouching(posX, posY, "F") && currentMap != 7))
                {
                    BuildChar(this.posX, this.posY, direction);

                    UpdateRow(this.posY - 1);
                    UpdateRow(this.posY);
                    UpdateRow(this.posY + 1);
                    UpdateRow(this.posY + 2);

                    Hit();
                }
                else if (currentMap == 12 && IsTouching(posX, posY, "X") && !IsTouching(posX, posY, "="))
                {
                    BuildChar(this.posX, this.posY, direction);

                    UpdateRow(this.posY - 1);
                    UpdateRow(this.posY);
                    UpdateRow(this.posY + 1);
                    UpdateRow(this.posY + 2);

                    LoadMap(9, 50, 9, "s");
                }
                else
                {
                    BuildChar(this.posX, this.posY, direction);

                    UpdateRow(this.posY - 1);
                    UpdateRow(this.posY);
                    UpdateRow(this.posY + 1);
                    UpdateRow(this.posY + 2);
                }
            }
            else
            {
                if (currentMap == 1)
                {
                    LoadMap(0, 63, 1, "s");
                }
                else if (currentMap == 4)
                {
                    if (posX > 29)
                    {
                        LoadMap(2, 55, 1, "s");
                    }
                    else if (posX == 21)
                    {
                        LoadMap(2, 21, 2, "s");
                    }
                    else
                    {
                        LoadMap(2, 10, 2, "s");
                    }
                }
                else if (currentMap == 5)
                {
                    LoadMap(3, 49, 2, "s");
                }
                else if (currentMap == 6)
                {
                    LoadMap(0, 16, 6, "s");
                    Wait(2);
                }
                else if (currentMap == 7)
                {
                    LoadMap(4, 86, 7, "s");
                    Wait(2);
                }
                else if (currentMap == 9)
                {
                    LoadMap(8, 51, 17, "s");
                    Wait(2);
                }
            }
            cText = false;
        }
        else if (direction == "d")
        {
            prev2 = "d";
            if (posX <= 99)
            {
                IsTouching(posX, posY, "r");
                StoreChar(this.posX, this.posY);

                bool persist = true;
                if (currentMap == 6 && (IsTouching(posX, posY, "-") || IsTouching(posX, posY, "S")))
                {
                    hasSword = true;
                    LoadMap(6, posX, posY, direction);
                }
                else if (currentMap == 7 && IsTouching(posX, posY, "*") && rupees >= 35)
                {
                    rupees -= 35;

                    hasRaft = true;
                    LoadMap(7, posX, posY, direction);
                }
                else if (currentMap == 7 && IsTouching(posX, posY, "Y") && rupees >= 10)
                {
                    rupees -= 10;
                    keys++;
                }
                else if (currentMap == 7 && IsTouching(posX, posY, "#") && rupees >= 25)
                {
                    rupees -= 25;

                    hasArmor = true;
                    LoadMap(7, posX, posY, direction);
                }
                else if (currentMap == 9 && IsTouching(posX, posY, "X") && !IsTouching(posX, posY, "=") && cDoor2 && !debounce)
                {
                    persist = false;
                    LoadMap(11, 14, 15, direction);
                }
                else if (currentMap == 9 && this.posX == 86 && this.posY >= 14 && this.posY <= 16 && !cDoor2 && keys > 0)
                {
                    debounce = true;
                    keys--;

                    cDoor2 = true;
                    LoadMap(9, this.posX, this.posY, direction);
                }

                if (!IsTouching(posX, posY, "=") && !IsTouching(posX, posY, "X") && !IsTouching(posX, posY, "t") && !IsTouching(posX, posY, "n") && !IsTouching(posX, posY, "B") && !IsTouching(posX, posY, "{") && !IsTouching(posX, posY, "}") && !IsTouching(posX, posY, "S") && !IsTouching(posX, posY, "<") && !(IsTouching(posX, posY, "F") && currentMap != 7) && !IsTouching(posX, posY, "~") && ((currentMap >= 9 && !IsTouching(posX, posY, "/")) || currentMap < 9) && persist)
                {
                    StoreChar(posX, posY);
                    BuildChar(posX, posY, direction);

                    UpdateRow(posY - 1);
                    UpdateRow(posY);
                    UpdateRow(posY + 1);
                    UpdateRow(posY + 2);

                    this.posX = posX;
                    this.posY = posY;
                }
                else if (IsTouching(posX, posY, "t") || IsTouching(posX, posY, "n") || IsTouching(posX, posY, "B") || IsTouching(posX, posY, "{") || IsTouching(posX, posY, "}") || IsTouching(posX, posY, "S") || IsTouching(posX, posY, "<") || (IsTouching(posX, posY, "F") && currentMap != 7))
                {
                    BuildChar(this.posX, this.posY, direction);

                    UpdateRow(this.posY - 1);
                    UpdateRow(this.posY);
                    UpdateRow(this.posY + 1);
                    UpdateRow(this.posY + 2);

                    Hit();
                }
                else if (IsTouching(posX, posY, "~") && this.posX != 21 && hasRaft && !IsTouching(posX, posY, "=") && !IsTouching(posX, posY, "X") && !IsTouching(posX, posY, "t") && !IsTouching(posX, posY, "n"))
                {
                    StoreChar(this.posX, this.posY);

                    UpdateRow(this.posY - 1);
                    UpdateRow(this.posY);
                    UpdateRow(this.posY + 1);
                    UpdateRow(this.posY + 2);

                    this.posX = 21;
                    DeployRaft("d");
                    wait = 150;
                }
                else if (this.posX == 21 && posY < 25 && ((posY > 3 && currentMap == 2) || (posY < 25 && currentMap == 4)))
                {
                    BuildChar(this.posX, this.posY, direction);

                    UpdateRow(this.posY - 1);
                    UpdateRow(this.posY);
                    UpdateRow(this.posY + 1);
                    UpdateRow(this.posY + 2);

                    for (int y = this.posY - 2; y <= this.posY + 3; y++)
                    {
                        for (int x = this.posX - 3; x <= this.posX + 3; x++)
                        {
                            map[x, y] = "~";
                        }
                    }

                    this.posX = 30;
                    posX = 30;

                    BuildChar(posX, posY, direction);

                    UpdateRow(posY - 2);
                    UpdateRow(posY - 1);
                    UpdateRow(posY);
                    UpdateRow(posY + 1);
                    UpdateRow(posY + 2);
                    UpdateRow(posY + 3);
                }
                else if (currentMap == 10 && IsTouching(posX, posY, "X") && !IsTouching(posX, posY, "="))
                {
                    LoadMap(9, 14, 15, "d");
                }
                else if (currentMap == 12 && IsTouching(posX, posY, "X") && !IsTouching(posX, posY, "="))
                {
                    LoadMap(13, 15, 15, "d");
                }
                else
                {
                    BuildChar(this.posX, this.posY, direction);

                    UpdateRow(this.posY - 1);
                    UpdateRow(this.posY);
                    UpdateRow(this.posY + 1);
                    UpdateRow(this.posY + 2);
                }
            }
            else
            {
                if (currentMap == 2)
                {
                    LoadMap(0, 4, 12, "d");
                }
                else if (currentMap == 0)
                {
                    LoadMap(3, 2, 18, "d");
                }
                else if (currentMap == 4)
                {
                    LoadMap(1, 2, 13, "d");
                }
                else if (currentMap == 1)
                {
                    LoadMap(5, 2, 15, "d");
                }
                else if (currentMap == 8)
                {
                    LoadMap(4, 2, 16, "d");
                }
            }
            if (cText)
            {
                cText = false;
                LoadMap(9, posX, posY, "w");
            }
        }
        debounce = false;
    }

    public void BuildChar(int posX, int posY, string direction)
    {
        string spaceslot = " ";
        string underslot = "_";
        if (hasArmor)
        {
            spaceslot = "#";
            underslot = "#";
        }

        if (direction == "w")
        {
            map[posX - 2, posY - 1] = " ";
            map[posX - 1, posY - 1] = "_";
            map[posX, posY - 1] = "_";
            map[posX + 1, posY - 1] = "_";
            map[posX + 2, posY - 1] = " ";

            map[posX - 2, posY] = "|";
            map[posX - 1, posY] = spaceslot;
            map[posX, posY] = "=";
            map[posX + 1, posY] = spaceslot;
            map[posX + 2, posY] = "|";

            map[posX - 2, posY + 1] = "|";
            map[posX - 1, posY + 1] = "^";
            map[posX, posY + 1] = spaceslot;
            map[posX + 1, posY + 1] = "^";
            map[posX + 2, posY + 1] = "|";

            map[posX - 2, posY + 2] = " ";
            map[posX - 1, posY + 2] = "\\";
            map[posX, posY + 2] = "_";
            map[posX + 1, posY + 2] = "/";
            map[posX + 2, posY + 2] = " ";
        }
        else if (direction == "a")
        {
            map[posX - 2, posY - 1] = " ";
            map[posX - 1, posY - 1] = " ";
            map[posX, posY - 1] = "/";
            map[posX + 1, posY - 1] = "\\";
            map[posX + 2, posY - 1] = " ";

            map[posX - 2, posY] = " ";
            map[posX - 1, posY] = "/";
            map[posX, posY] = " ";
            map[posX + 1, posY] = " ";
            map[posX + 2, posY] = "|";

            map[posX - 2, posY + 1] = "|";
            map[posX - 1, posY + 1] = "^";
            map[posX, posY + 1] = spaceslot;
            map[posX + 1, posY + 1] = spaceslot;
            map[posX + 2, posY + 1] = "|";

            map[posX - 2, posY + 2] = "|";
            map[posX - 1, posY + 2] = underslot;
            map[posX, posY + 2] = "=";
            map[posX + 1, posY + 2] = underslot;
            map[posX + 2, posY + 2] = "|";
        }
        else if (direction == "s")
        {
            map[posX - 2, posY - 1] = " ";
            map[posX - 1, posY - 1] = " ";
            map[posX, posY - 1] = "_";
            map[posX + 1, posY - 1] = " ";
            map[posX + 2, posY - 1] = " ";

            map[posX - 2, posY] = " ";
            map[posX - 1, posY] = "/";
            map[posX, posY] = " ";
            map[posX + 1, posY] = "\\";
            map[posX + 2, posY] = " ";

            map[posX - 2, posY + 1] = "|";
            map[posX - 1, posY + 1] = "^";
            map[posX, posY + 1] = spaceslot;
            map[posX + 1, posY + 1] = "^";
            map[posX + 2, posY + 1] = "|";

            map[posX - 2, posY + 2] = "|";
            map[posX - 1, posY + 2] = underslot;
            map[posX, posY + 2] = "=";
            map[posX + 1, posY + 2] = underslot;
            map[posX + 2, posY + 2] = "|";
        }
        else if (direction == "d")
        {
            map[posX - 2, posY - 1] = " ";
            map[posX - 1, posY - 1] = "/";
            map[posX, posY - 1] = "\\";
            map[posX + 1, posY - 1] = " ";
            map[posX + 2, posY - 1] = " ";

            map[posX - 2, posY] = "|";
            map[posX - 1, posY] = " ";
            map[posX, posY] = " ";
            map[posX + 1, posY] = "\\";
            map[posX + 2, posY] = " ";

            map[posX - 2, posY + 1] = "|";
            map[posX - 1, posY + 1] = spaceslot;
            map[posX, posY + 1] = spaceslot;
            map[posX + 1, posY + 1] = "^";
            map[posX + 2, posY + 1] = "|";

            map[posX - 2, posY + 2] = "|";
            map[posX - 1, posY + 2] = underslot;
            map[posX, posY + 2] = "=";
            map[posX + 1, posY + 2] = underslot;
            map[posX + 2, posY + 2] = "|";
        }
    }

    public void StoreChar(int posX, int posY)
    {
        map[this.posX - 2, this.posY - 1] = storage_map[0];
        map[this.posX - 1, this.posY - 1] = storage_map[1];
        map[this.posX, this.posY - 1] = storage_map[2];
        map[this.posX + 1, this.posY - 1] = storage_map[3];
        map[this.posX + 2, this.posY - 1] = storage_map[4];

        map[this.posX - 2, this.posY] = storage_map[5];
        map[this.posX - 1, this.posY] = storage_map[6];
        map[this.posX, this.posY] = storage_map[7];
        map[this.posX + 1, this.posY] = storage_map[8];
        map[this.posX + 2, this.posY] = storage_map[9];

        map[this.posX - 2, this.posY + 1] = storage_map[10];
        map[this.posX - 1, this.posY + 1] = storage_map[11];
        map[this.posX, this.posY + 1] = storage_map[12];
        map[this.posX + 1, this.posY + 1] = storage_map[13];
        map[this.posX + 2, this.posY + 1] = storage_map[14];

        map[this.posX - 2, this.posY + 2] = storage_map[15];
        map[this.posX - 1, this.posY + 2] = storage_map[16];
        map[this.posX, this.posY + 2] = storage_map[17];
        map[this.posX + 1, this.posY + 2] = storage_map[18];
        map[this.posX + 2, this.posY + 2] = storage_map[19];

        UpdateRow(this.posY - 1);
        UpdateRow(this.posY);
        UpdateRow(this.posY + 1);
        UpdateRow(this.posY + 2);

        storage_map[0] = map[posX - 2, posY - 1];
        storage_map[1] = map[posX - 1, posY - 1];
        storage_map[2] = map[posX, posY - 1];
        storage_map[3] = map[posX + 1, posY - 1];
        storage_map[4] = map[posX + 2, posY - 1];

        storage_map[5] = map[posX - 2, posY];
        storage_map[6] = map[posX - 1, posY];
        storage_map[7] = map[posX, posY];
        storage_map[8] = map[posX + 1, posY];
        storage_map[9] = map[posX + 2, posY];

        storage_map[10] = map[posX - 2, posY + 1];
        storage_map[11] = map[posX - 1, posY + 1];
        storage_map[12] = map[posX, posY + 1];
        storage_map[13] = map[posX + 1, posY + 1];
        storage_map[14] = map[posX + 2, posY + 1];

        storage_map[15] = map[posX - 2, posY + 2];
        storage_map[16] = map[posX - 1, posY + 2];
        storage_map[17] = map[posX, posY + 2];
        storage_map[18] = map[posX + 1, posY + 2];
        storage_map[19] = map[posX + 2, posY + 2];
    }

    public void DeployRaft(string direction)
    {
        string spaceslot = " ";
        string underslot = "_";
        if (hasArmor)
        {
            spaceslot = "#";
            underslot = "#";
        }

        map[posX - 3, posY - 2] = "*";
        map[posX - 2, posY - 2] = "*";
        map[posX - 1, posY - 2] = "*";
        map[posX, posY - 2] = "*";
        map[posX + 1, posY - 2] = "*";
        map[posX + 2, posY - 2] = "*";
        map[posX + 3, posY - 2] = "*";

        map[posX - 3, posY - 1] = "=";
        map[posX, posY - 1] = " ";
        map[posX + 3, posY - 1] = "=";

        map[posX - 3, posY] = "*";
        map[posX - 2, posY] = "|";
        map[posX + 2, posY] = "|";
        map[posX + 3, posY] = "*";

        map[posX - 3, posY + 1] = "*";
        map[posX - 2, posY + 1] = "|";
        map[posX - 1, posY + 1] = underslot;
        map[posX, posY + 1] = "=";
        map[posX + 1, posY + 1] = underslot;
        map[posX + 2, posY + 1] = "|";
        map[posX + 3, posY + 1] = "*";

        map[posX - 3, posY + 2] = "=";
        map[posX - 2, posY + 2] = "=";
        map[posX - 1, posY + 2] = "=";
        map[posX, posY + 2] = "=";
        map[posX + 1, posY + 2] = "=";
        map[posX + 2, posY + 2] = "=";
        map[posX + 3, posY + 2] = "=";

        map[posX - 3, posY + 3] = "*";
        map[posX - 2, posY + 3] = "*";
        map[posX - 1, posY + 3] = "*";
        map[posX, posY + 3] = "*";
        map[posX + 1, posY + 3] = "*";
        map[posX + 2, posY + 3] = "*";
        map[posX + 3, posY + 3] = "*";

        if (direction == "a")
        {
            map[posX - 2, posY - 1] = "=";
            map[posX - 1, posY - 1] = "/";
            map[posX + 1, posY - 1] = " ";
            map[posX + 2, posY - 1] = "|";

            map[posX - 1, posY] = "^";
            map[posX, posY] = spaceslot;
            map[posX + 1, posY] = spaceslot;
        }
        else if (direction == "d")
        {
            map[posX - 2, posY - 1] = "|";
            map[posX - 1, posY - 1] = " ";
            map[posX + 1, posY - 1] = "\\";
            map[posX + 2, posY - 1] = "=";

            map[posX - 1, posY] = spaceslot;
            map[posX, posY] = spaceslot;
            map[posX + 1, posY] = "^";
        }

        UpdateRow(posY - 2);
        UpdateRow(posY - 1);
        UpdateRow(posY);
        UpdateRow(posY + 1);
        UpdateRow(posY + 2);
        UpdateRow(posY + 3);
    }

    public void PlayEffect(string symbol)
    {
        map[posX - 2, posY - 1] = symbol;
        map[posX - 1, posY - 1] = symbol;
        map[posX, posY - 1] = symbol;
        map[posX + 1, posY - 1] = symbol;
        map[posX + 2, posY - 1] = symbol;

        map[posX - 2, posY] = symbol;
        map[posX - 1, posY] = symbol;
        map[posX, posY] = symbol;
        map[posX + 1, posY] = symbol;
        map[posX + 2, posY] = symbol;

        map[posX - 2, posY + 1] = symbol;
        map[posX - 1, posY + 1] = symbol;
        map[posX, posY + 1] = symbol;
        map[posX + 1, posY + 1] = symbol;
        map[posX + 2, posY + 1] = symbol;

        map[posX - 2, posY + 2] = symbol;
        map[posX - 1, posY + 2] = symbol;
        map[posX, posY + 2] = symbol;
        map[posX + 1, posY + 2] = symbol;
        map[posX + 2, posY + 2] = symbol;
    }

    public void PlaceZelda()
    {
        map[50, 14] = "=";
        map[51, 14] = "<";
        map[52, 14] = ">";
        map[53, 14] = "=";

        map[50, 15] = "s";
        map[51, 15] = "^";
        map[52, 15] = "^";
        map[53, 15] = "s";

        map[49, 16] = "s";
        map[50, 16] = "s";
        map[51, 16] = "~";
        map[52, 16] = "~";
        map[53, 16] = "s";
        map[54, 16] = "s";

        map[49, 16] = "~";
        map[50, 16] = "~";
        map[51, 16] = "~";
        map[52, 16] = "~";
        map[53, 16] = "~";
        map[54, 16] = "~";
    }

    public void Hit()
    {
        if (iFrames <= 0)
        {
            iFrames = 6;

            if (hasArmor)
            {
                health -= 0.5;
            }
            else
            {
                health--;
            }
            hit = true;

            StoreChar(posX, posY);

            PlayEffect("*");

            UpdateRow(posY - 1);
            UpdateRow(posY);
            UpdateRow(posY + 1);
            UpdateRow(posY + 2);
        }
    }

    public bool IsTouching(int posX, int posY, string symbol)
    {
        if (symbol == "/")
        {
            if (map[posX, posY - 1] == "/")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        if (map[posX - 2, posY - 1] == symbol || map[posX - 1, posY - 1] == symbol || map[posX, posY - 1] == symbol || map[posX + 1, posY - 1] == symbol || map[posX + 2, posY - 1] == symbol || map[posX - 2, posY] == symbol || map[posX - 1, posY] == symbol || map[posX, posY] == symbol || map[posX + 1, posY] == symbol || map[posX + 2, posY] == symbol || map[posX - 2, posY + 1] == symbol || map[posX - 1, posY + 1] == symbol || map[posX, posY + 1] == symbol || map[posX + 1, posY + 1] == symbol || map[posX + 2, posY + 1] == symbol || map[posX - 2, posY + 2] == symbol || map[posX - 1, posY + 2] == symbol || map[posX, posY + 2] == symbol || map[posX + 1, posY + 2] == symbol || map[posX + 2, posY + 2] == symbol)
        {
            if (symbol == "R" || symbol == "r")
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (map[posX - 2 + j, posY - 1 + i] == "R" || map[posX - 2 + j, posY - 1 + i] == "r")
                        {
                            enemyMovement.RemoveRupee(posX - 2 + j, posY - 1 + i);
                        }
                    }
                }
            }
            return true;
        }
        return false;
    }
}
