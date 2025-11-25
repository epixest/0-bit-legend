using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace ZeldaEmulator;

public class EnemyMovement : MainProgram
{
    // Enemy
    private List<int> posX = new List<int>();
    private List<int> posY = new List<int>();

    private List<string[]> map_storage = new List<string[]>();

    private List<string> prev1 = new List<string>();
    private List<string> prev2 = new List<string>();

    private List<string> type = new List<string>();
    private List<int> hp = new List<int>();

    private List<int> motion = new List<int>();

    // Rupee
    private List<int> rPosX = new List<int>();
    private List<int> rPosY = new List<int>();
    private List<string[]> rupee_storage = new List<string[]>();

    private int sRPosX;
    private int sRPosY;
    private string sRType = "";

    //    Methods    //
    public int GetPosX(int index)
    {
        return posX[index];
    }

    public int GetPosY(int index)
    {
        return posY[index];
    }

    public string GetEnemyType(int index)
    {
        return type[index];
    }

    public int GetTotal()
    {
        return posX.ToArray().Length;
    }

    public string GetPrev1(int index)
    {
        return prev1[index];
    }

    public string GetPrev2(int index)
    {
        return prev2[index];
    }

    public int GetMotion(int index)
    {
        return motion[index];
    }

    public void SetMotion(int index, int value)
    {
        motion[index] = value;
    }


    public bool TakeDamage(int posX, int posY, string prev, int damage)
    {
        int index = GetIndex(posX, posY);
        linkMovement.StoreSword(prev);

        if (index == -1 || type[index] == "fireball")
        {
            return false;
        }

        if (type[index] == "dragon")
        {
            waitDragon++;

            int value = 0;
            string dragon = "*****        ******      **  ***        ***        *********   ********     ***  ** ";
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    map[GetPosX(index) + j, GetPosY(index) + i] = dragon[value].ToString();
                    value++;
                }
            }
        }

        hp[index] -= 1;
        if (hp[index] <= 0)
        {
            sRPosX = this.posX[index] + 2;
            sRPosY = this.posY[index] + 1;
            sRType = type[index];

            Remove(index, type[index]);

            linkMovement.SetSpawnRupee(true);

            if (sRType == "dragon")
            {
                cDragon = true;
                LoadMap(12, linkMovement.GetPosX(), linkMovement.GetPosY(), linkMovement.GetPrev());
            }
        }
        return true;
    }

    public bool Move(int index, string type, int posX, int posY, string direction, int motion, bool spawn)
    {
        if (index == -1)
        {
            index = GetTotal();
        }

        if (spawn)
        {
            this.posX.Add(posX);
            this.posY.Add(posY);

            prev1.Add(direction);
            prev2.Add(direction);

            this.type.Add(type);
            this.hp.Add(1);

            this.motion.Add(motion);

            string[] storage_copy;
            if (type == "octorok")
            {
                storage_copy = new string[12];
            }
            else if (type == "spider")
            {
                storage_copy = new string[15];
            }
            else if (type == "bat")
            {
                storage_copy = new string[10];
            }
            else if (type == "dragon")
            {
                storage_copy = new string[84];
                hp[GetTotal() - 1] = 3;
            }
            else if (type == "fireball")
            {
                storage_copy = new string[6];
            }
            else
            {
                storage_copy = new string[12];
            }

            for (int i = 0; i < storage_copy.Length; i++)
            {
                storage_copy[i] = " ";
            }

            map_storage.Add(storage_copy);
        }

        if (InBounds(type, posX, posY))
        {
            Clear(index, type);
            if (type == "dragon" || (type == "spider" || type == "bat" || (!IsTouching(type, posX, posY, "=") && !IsTouching(type, posX, posY, "X"))) && !IsTouching(type, posX, posY, "t") && !IsTouching(type, posX, posY, "n") && !IsTouching(type, posX, posY, "B") && !IsTouching(type, posX, posY, "{") && !IsTouching(type, posX, posY, "}") && !IsTouching(type, posX, posY, "|") && !IsTouching(type, posX, posY, "/") && !IsTouching(type, posX, posY, "\\") && !IsTouching(type, posX, posY, "_") && !IsTouching(type, posX, posY, "~"))
            {
                prev1[index] = direction;

                if (type == "octorok")
                {
                    if (direction == "a" || direction == "d")
                    {
                        prev2[index] = direction;
                    }
                }
                else if (type == "spider")
                {
                    if (direction == "w" || direction == "s")
                    {
                        prev2[index] = "a";
                    }
                    else if (direction == "a" || direction == "d")
                    {
                        prev2[index] = "d";
                    }
                }
                else if (type == "bat")
                {
                    if (prev2[index] == "d")
                    {
                        prev2[index] = "a";
                    }
                    else if (prev2[index] == "a")
                    {
                        prev2[index] = "d";
                    }
                }

                Store(index, type, posX, posY);
                Build(index, type, posX, posY);

                UpdateRow(posY);
                UpdateRow(posY + 1);
                UpdateRow(posY + 2);

                if (type == "dragon")
                {
                    UpdateRow(posY + 3);
                    UpdateRow(posY + 4);
                    UpdateRow(posY + 5);
                    UpdateRow(posY + 6);
                }

                this.posX[index] = posX;
                this.posY[index] = posY;

                return true;
            }
            else if (IsTouching(type, posX, posY, "|") || IsTouching(type, posX, posY, "_") || IsTouching(type, posX, posY, "\\"))
            {
                linkMovement.Hit();
                if (type == "fireball")
                {
                    Remove(GetIndex(this.posX[index], this.posY[index]), type);
                }
                else
                {
                    Build(index, type, this.posX[index], this.posY[index]);
                }
            }
            else
            {
                if (type == "fireball")
                {
                    Remove(GetIndex(this.posX[index], this.posY[index]), type);
                }
                else
                {
                    Build(index, type, this.posX[index], this.posY[index]);
                }
            }
        }
        return false;
    }

    public void Build(int index, string type, int posX, int posY)
    {
        if (prev2[index] == "a")
        {
            if (type == "octorok")
            {
                map[posX + 0, posY] = " ";
                map[posX + 1, posY] = "t";
                map[posX + 2, posY] = "t";
                map[posX + 3, posY] = "t";

                map[posX + 0, posY + 1] = "t";
                map[posX + 1, posY + 1] = "^";
                map[posX + 2, posY + 1] = "t";
                map[posX + 3, posY + 1] = "t";

                map[posX + 0, posY + 2] = "t";
                map[posX + 1, posY + 2] = "t";
                map[posX + 2, posY + 2] = "t";
                map[posX + 3, posY + 2] = "t";
            }
            else if (type == "spider")
            {
                map[posX + 0, posY] = " ";
                map[posX + 1, posY] = "t";
                map[posX + 2, posY] = "t";
                map[posX + 3, posY] = "t";
                map[posX + 4, posY] = " ";

                map[posX + 0, posY + 1] = "n";
                map[posX + 1, posY + 1] = "0";
                map[posX + 2, posY + 1] = "0";
                map[posX + 3, posY + 1] = "t";
                map[posX + 4, posY + 1] = "t";

                map[posX + 0, posY + 2] = " ";
                map[posX + 1, posY + 2] = "n";
                map[posX + 2, posY + 2] = "t";
                map[posX + 4, posY + 2] = " ";
                map[posX + 3, posY + 2] = "n";
            }
            else if (type == "bat")
            {
                map[posX + 0, posY] = "{";
                map[posX + 1, posY] = "t";
                map[posX + 2, posY] = " ";
                map[posX + 3, posY] = "t";
                map[posX + 4, posY] = "}";

                map[posX + 0, posY + 1] = " ";
                map[posX + 1, posY + 1] = " ";
                map[posX + 2, posY + 1] = "B";
                map[posX + 3, posY + 1] = " ";
                map[posX + 4, posY + 1] = " ";
            }
        }
        else
        {
            if (type == "octorok")
            {
                map[posX + 0, posY] = "t";
                map[posX + 1, posY] = "t";
                map[posX + 2, posY] = "t";
                map[posX + 3, posY] = " ";

                map[posX + 0, posY + 1] = "t";
                map[posX + 1, posY + 1] = "t";
                map[posX + 2, posY + 1] = "^";
                map[posX + 3, posY + 1] = "t";

                map[posX + 0, posY + 2] = "t";
                map[posX + 1, posY + 2] = "t";
                map[posX + 2, posY + 2] = "t";
                map[posX + 3, posY + 2] = "t";
            }
            else if (type == "spider")
            {
                map[posX + 0, posY] = " ";
                map[posX + 1, posY] = "t";
                map[posX + 2, posY] = "t";
                map[posX + 3, posY] = "t";
                map[posX + 4, posY] = " ";

                map[posX + 0, posY + 1] = "t";
                map[posX + 1, posY + 1] = "t";
                map[posX + 2, posY + 1] = "0";
                map[posX + 3, posY + 1] = "0";
                map[posX + 4, posY + 1] = "n";

                map[posX + 0, posY + 2] = " ";
                map[posX + 1, posY + 2] = "n";
                map[posX + 2, posY + 2] = "t";
                map[posX + 3, posY + 2] = "n";
                map[posX + 4, posY + 2] = " ";
            }
            else if (type == "bat")
            {
                map[posX + 0, posY] = " ";
                map[posX + 1, posY] = " ";
                map[posX + 2, posY] = "B";
                map[posX + 3, posY] = " ";
                map[posX + 4, posY] = " ";

                map[posX + 0, posY + 1] = "{";
                map[posX + 1, posY + 1] = "t";
                map[posX + 2, posY + 1] = " ";
                map[posX + 3, posY + 1] = "t";
                map[posX + 4, posY + 1] = "}";
            }
        }
        if (type == "dragon")
        {
            string dragon = "<***>        S^SSS>      *S  SS>        =S>        =*SSSS**>   =*SSSSS*     ===  == ";
            if (prev1[index] == "d") dragon = "<***>        F^FFF>      *F  FS>        FF>        FF*SSS**>   F**SSSS*     ===  == ";

            bool debounce = false;
            int value = 0;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    if (map[posX + j, posY + i] == "/" || map[posX + j, posY + i] == "\\" || map[posX + j, posY + i] == "|" || map[posX + j, posY + i] == "_" && !debounce)
                    {
                        debounce = true;
                        linkMovement.Hit();
                    }
                    map[posX + j, posY + i] = dragon[value].ToString();
                    value++;
                }
            }
        }
        else if (type == "fireball")
        {
            map[posX + 0, posY] = "F";
            map[posX + 1, posY] = "F";
            map[posX + 2, posY] = "F";

            map[posX + 0, posY + 1] = "F";
            map[posX + 1, posY + 1] = "F";
            map[posX + 2, posY + 1] = "F";
        }
    }

    public void Store(int index, string type, int posX, int posY)
    {
        Clear(index, type);

        if (type == "octorok")
        {
            int value = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    map_storage[index][value] = map[posX + j, posY + i];
                    value++;
                }
            }
        }
        else if (type == "spider")
        {
            int value = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    map_storage[index][value] = map[posX + j, posY + i];
                    value++;
                }
            }
        }
        else if (type == "bat")
        {
            int value = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    map_storage[index][value] = map[posX + j, posY + i];
                    value++;
                }
            }
        }
        else if (type == "fireball")
        {
            int value = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    map_storage[index][value] = map[posX + j, posY + i];
                    value++;
                }
            }
        }

        for (int i = 0; i < map_storage[index].Length; i++)
        {
            if (map_storage[index][i] == "*" || map_storage[index][i] == "F" || map_storage[index][i] == "S" || map_storage[index][i] == "-" || map_storage[index][i] == "/" || map_storage[index][i] == "\\" || map_storage[index][i] == "|" || map_storage[index][i] == "^" || map_storage[index][i] == "#" || map_storage[index][i] == "r" || map_storage[index][i] == "R" || map_storage[index][i] == "V")
            {
                map_storage[index][i] = " ";
            }
        }

        UpdateRow(this.posY[index]);
        UpdateRow(this.posY[index] + 1);
        UpdateRow(this.posY[index] + 2);

        if (type == "dragon")
        {
            UpdateRow(this.posY[index] + 3);
            UpdateRow(this.posY[index] + 4);
            UpdateRow(this.posY[index] + 5);
            UpdateRow(this.posY[index] + 6);
        }
    }

    public void Clear(int index, string type)
    {
        if (type == "octorok")
        {
            map[posX[index] + 0, posY[index]] = map_storage[index][0];
            map[posX[index] + 1, posY[index]] = map_storage[index][1];
            map[posX[index] + 2, posY[index]] = map_storage[index][2];
            map[posX[index] + 3, posY[index]] = map_storage[index][3];

            map[posX[index] + 0, posY[index] + 1] = map_storage[index][4];
            map[posX[index] + 1, posY[index] + 1] = map_storage[index][5];
            map[posX[index] + 2, posY[index] + 1] = map_storage[index][6];
            map[posX[index] + 3, posY[index] + 1] = map_storage[index][7];

            map[posX[index] + 0, posY[index] + 2] = map_storage[index][8];
            map[posX[index] + 1, posY[index] + 2] = map_storage[index][9];
            map[posX[index] + 2, posY[index] + 2] = map_storage[index][10];
            map[posX[index] + 3, posY[index] + 2] = map_storage[index][11];
        }
        else if (type == "spider")
        {
            map[posX[index] + 0, posY[index]] = map_storage[index][0];
            map[posX[index] + 1, posY[index]] = map_storage[index][1];
            map[posX[index] + 2, posY[index]] = map_storage[index][2];
            map[posX[index] + 3, posY[index]] = map_storage[index][3];
            map[posX[index] + 4, posY[index]] = map_storage[index][4];

            map[posX[index] + 0, posY[index] + 1] = map_storage[index][5];
            map[posX[index] + 1, posY[index] + 1] = map_storage[index][6];
            map[posX[index] + 2, posY[index] + 1] = map_storage[index][7];
            map[posX[index] + 3, posY[index] + 1] = map_storage[index][8];
            map[posX[index] + 4, posY[index] + 1] = map_storage[index][9];

            map[posX[index] + 0, posY[index] + 2] = map_storage[index][10];
            map[posX[index] + 1, posY[index] + 2] = map_storage[index][11];
            map[posX[index] + 2, posY[index] + 2] = map_storage[index][12];
            map[posX[index] + 3, posY[index] + 2] = map_storage[index][13];
            map[posX[index] + 4, posY[index] + 2] = map_storage[index][14];
        }
        else if (type == "bat")
        {
            map[posX[index] + 0, posY[index]] = map_storage[index][0];
            map[posX[index] + 1, posY[index]] = map_storage[index][1];
            map[posX[index] + 2, posY[index]] = map_storage[index][2];
            map[posX[index] + 3, posY[index]] = map_storage[index][3];
            map[posX[index] + 4, posY[index]] = map_storage[index][4];

            map[posX[index] + 0, posY[index] + 1] = map_storage[index][5];
            map[posX[index] + 1, posY[index] + 1] = map_storage[index][6];
            map[posX[index] + 2, posY[index] + 1] = map_storage[index][7];
            map[posX[index] + 3, posY[index] + 1] = map_storage[index][8];
            map[posX[index] + 4, posY[index] + 1] = map_storage[index][9];
        }
        else if (type == "dragon")
        {
            int value = 0;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    map[posX[index] + j, posY[index] + i] = " ";
                    value++;
                }
            }
        }
        else if (type == "fireball")
        {
            map[posX[index] + 0, posY[index]] = map_storage[index][0];
            map[posX[index] + 1, posY[index]] = map_storage[index][1];
            map[posX[index] + 2, posY[index]] = map_storage[index][2];

            map[posX[index] + 0, posY[index] + 1] = map_storage[index][3];
            map[posX[index] + 1, posY[index] + 1] = map_storage[index][4];
            map[posX[index] + 2, posY[index] + 1] = map_storage[index][5];
        }
    }

    public void Remove(int index, string type)
    {
        Clear(index, type);

        UpdateRow(posY[index]);
        UpdateRow(posY[index] + 1);
        UpdateRow(posY[index] + 2);

        if (type == "dragon")
        {
            UpdateRow(posY[index] + 3);
            UpdateRow(posY[index] + 4);
            UpdateRow(posY[index] + 5);
            UpdateRow(posY[index] + 6);
        }

        posX.RemoveAt(index);
        posY.RemoveAt(index);
        this.type.RemoveAt(index);
        prev1.RemoveAt(index);
        prev2.RemoveAt(index);
        hp.RemoveAt(index);
        motion.RemoveAt(index);
        map_storage.RemoveAt(index);

        if (type == "bat")
        {
            if (currentMap == 10)
            {
                cEnemies1--;
            }
            else if (currentMap == 11)
            {
                cEnemies2--;
            }
        }
    }

    public void SpawnRupee()
    {
        if (sRType != "dragon" && sRType != "bat" && new Random().Next(2) == 1)
        {
            string[] rupee_storage_copy = new string[9];

            int value = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (map[sRPosX - 1 + j, sRPosY - 1 + i] != "-" && map[sRPosX - 1 + j, sRPosY - 1 + i] != "S")
                    {
                        rupee_storage_copy[value] = map[sRPosX - 1 + j, sRPosY - 1 + i];
                    }
                    else
                    {
                        rupee_storage_copy[value] = " ";
                    }
                    value++;
                }
            }

            rPosX.Add(sRPosX);
            rPosY.Add(sRPosY);
            rupee_storage.Add(rupee_storage_copy);

            if (new Random().Next(5) == 4 || (sRType == "spider" && new Random().Next(10) == 9))
            {
                map[sRPosX, sRPosY] = "V";
            }
            else
            {
                map[sRPosX, sRPosY] = "R";
            }

            map[sRPosX - 1, sRPosY] = "R";
            map[sRPosX + 1, sRPosY] = "R";
            map[sRPosX, sRPosY - 1] = "r";
            map[sRPosX, sRPosY + 1] = "r";

            UpdateRow(sRPosY - 1);
            UpdateRow(sRPosY);
            UpdateRow(sRPosY + 1);
        }
    }

    public void RemoveRupee(int posX, int posY)
    {
        for (int i = 0; i < rPosX.ToArray().Length; i++)
        {
            if (posX >= this.rPosX[i] - 1 && posX <= this.rPosX[i] + 1 && posY >= this.rPosY[i] - 1 && posY <= this.rPosY[i] + 1)
            {
                if (map[rPosX[i], rPosY[i]] == "V")
                {
                    rupees += 5;
                }
                else
                {
                    rupees++;
                }

                map[rPosX[i] - 1, rPosY[i] - 1] = rupee_storage[i][0];
                map[rPosX[i] + 0, rPosY[i] - 1] = rupee_storage[i][1];
                map[rPosX[i] + 1, rPosY[i] - 1] = rupee_storage[i][2];

                map[rPosX[i] - 1, rPosY[i]] = rupee_storage[i][3];
                map[rPosX[i] + 0, rPosY[i]] = rupee_storage[i][4];
                map[rPosX[i] + 1, rPosY[i]] = rupee_storage[i][5];

                map[rPosX[i] - 1, rPosY[i] + 1] = rupee_storage[i][6];
                map[rPosX[i] + 0, rPosY[i] + 1] = rupee_storage[i][7];
                map[rPosX[i] + 1, rPosY[i] + 1] = rupee_storage[i][8];

                UpdateRow(rPosY[i] - 1);
                UpdateRow(rPosY[i]);
                UpdateRow(rPosY[i] + 1);

                rPosX.RemoveAt(i);
                rPosY.RemoveAt(i);
                rupee_storage.RemoveAt(i);
            }
        }
    }

    public bool InBounds(string type, int posX, int posY)
    {
        int inPosX = 0;
        int inPosY = 0;
        if (type == "octorok")
        {
            inPosX = posX + 3;
            inPosY = posY + 2;
        }
        else if (type == "spider")
        {
            inPosX = posX + 4;
            inPosY = posY + 2;
        }
        else if (type == "bat")
        {
            inPosX = posX + 4;
            inPosY = posY + 1;
        }
        else if (type == "fireball")
        {
            inPosX = posX + 3;
            inPosY = posY + 1;
        }

        if (posX <= 0 || inPosX >= 102 || posY <= 0 || inPosY >= 33)
        {
            return false;
        }
        return true;
    }

    public bool IsTouching(string type, int posX, int posY, string symbol)
    {
        if (type == "octorok" && (map[posX, posY] == symbol || map[posX + 1, posY] == symbol || map[posX + 2, posY] == symbol || map[posX + 3, posY] == symbol || map[posX, posY + 1] == symbol || map[posX + 1, posY + 1] == symbol || map[posX + 2, posY + 1] == symbol || map[posX + 3, posY + 1] == symbol || map[posX, posY + 2] == symbol || map[posX + 1, posY + 2] == symbol || map[posX + 2, posY + 2] == symbol || map[posX + 3, posY + 2] == symbol))
        {
            return true;
        }
        if (type == "spider" && (map[posX, posY] == symbol || map[posX + 1, posY] == symbol || map[posX + 2, posY] == symbol || map[posX + 3, posY] == symbol || map[posX + 4, posY] == symbol || map[posX, posY + 1] == symbol || map[posX + 1, posY + 1] == symbol || map[posX + 2, posY + 1] == symbol || map[posX + 3, posY + 1] == symbol || map[posX + 4, posY + 1] == symbol || map[posX, posY + 2] == symbol || map[posX + 1, posY + 2] == symbol || map[posX + 2, posY + 2] == symbol || map[posX + 3, posY + 2] == symbol || map[posX + 4, posY + 2] == symbol))
        {
            return true;
        }
        if (type == "bat" && (map[posX, posY] == symbol || map[posX + 1, posY] == symbol || map[posX + 2, posY] == symbol || map[posX + 3, posY] == symbol || map[posX + 4, posY] == symbol || map[posX, posY + 1] == symbol || map[posX + 1, posY + 1] == symbol || map[posX + 2, posY + 1] == symbol || map[posX + 3, posY + 1] == symbol || map[posX + 4, posY + 1] == symbol))
        {
            return true;
        }
        if (type == "fireball" && (map[posX, posY] == symbol || map[posX + 1, posY] == symbol || map[posX + 2, posY] == symbol || map[posX, posY + 1] == symbol || map[posX + 1, posY + 1] == symbol || map[posX + 2, posY + 1] == symbol))
        {
            return true;
        }
        return false;
    }

    public int GetIndex(int posX, int posY)
    {
        for (int i = 0; i < GetTotal(); i++)
        {
            int inPosX = 0;
            int inPosY = 0;
            if (type[i] == "octorok")
            {
                inPosX = 4;
                inPosY = 3;
            }
            else if (type[i] == "spider")
            {
                inPosX = 5;
                inPosY = 3;
            }
            else if (type[i] == "bat")
            {
                inPosX = 5;
                inPosY = 2;
            }
            else if (type[i] == "dragon")
            {
                inPosX = 12;
                inPosY = 7;
            }
            else if (type[i] == "fireball")
            {
                inPosX = 3;
                inPosY = 2;
            }

            if (posX >= this.posX[i] && posX <= this.posX[i] + inPosX && posY >= this.posY[i] && posY <= this.posY[i] + inPosY)
            {
                return i;
            }
        }
        return -1;
    }
}