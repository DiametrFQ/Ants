using System;

namespace ConsoleApp10
{
    class Program// БИЛЕТ # 42 
    {
        static Random rnd = new Random();

        public static int shrew = 7;

        public const int DROUGHTS = 14;

        static Heap[] Heaps = new Heap[]{
                new Heap(branchs: 30, stones: 48),
                new Heap(branchs: 17, sheets: 15),
                new Heap(branchs: 39, sheets: 24, stones: 20, dews: 22),
                new Heap(branchs: 22, sheets: 22, stones: 22),
                new Heap(branchs: 40, sheets: 30, stones: 30, dews: 21)
        };

        static Colony[] colonys = new Colony[] {
            new Colony("Черный",
                new Queen(color: "Черный", memory: 1, name: "Изабелла", health: 15, protect: 5, clas: "Элитный", damage: 24, cicle_First: 1, cicle_Last: 5, queen_First: 3, queen_Last: 3, mod_mather: true),
                new Worker[17],
                new Warior[7],
                new Special(color: "Черный",  memory: 1, health: 27, protect: 6, clas: "ленивый неуязвимый агрессивный неряшливый - Толстоножка", damage: 8, target: 3, bite: 3, mod_protect: true, mod_halfProtect: true)
            ),
             new Colony("Рыжий",
                new Queen(color: "Рыжий", memory: 2, name: "Изабелла", health: 23, protect: 7, clas: "Элитный", damage: 15, cicle_First: 3, cicle_Last: 3, queen_First: 3, queen_Last: 5, mod_mather: true),
                new Worker[17],
                new Warior[5],
                new Special(color: "Рыжий", memory: 2, health: 21, protect: 9, clas: "трудолюбивый обычный агрессивный дурной бригадир - Толстоножка", damage: 9, branch: 3, target: 3, bite: 1, mod_randomTrap: true, mod_doubleDamage: true, mod_plusRes: true)
            ),
        };

        static void Main(string[] args)
        {
            Console.WriteLine($"<<<<<<<<<<<<<<<<<<<<<<<<<<<< Начало >>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            Console.WriteLine();

            foreach (Colony colony in colonys)
            {
                for (int i = 0; i < colony.workers.Length; i++) colony.workers[i] = colony.WorkCall(colony.queen.memory);
                for (int i = 0; i < colony.wariors.Length; i++) colony.wariors[i] = colony.MilitaryConscription(colony.queen.memory);
            }
            foreach (Colony colony in colonys)
            {
                Console.WriteLine($"Колония №{colony.queen.memory} {colony.queen.color}:");
                colony.ResursesInColony();
                Console.WriteLine($"{" ",5}рабочие {colony.workers.Length}");
                Console.WriteLine($"{" ",5}войны {colony.wariors.Length }");
                Console.WriteLine($"{" ",5}особый {colony.special.damage != 0}");
                Console.WriteLine();
            }
            for (int i = 0; i < Heaps.Length; i++) Heaps[i].Resurses_in_heap(i);
            Console.WriteLine();

            for (int day = 0; day < DROUGHTS; day++)
            {
                Console.WriteLine($"<<<<<<<<<<<<<<<<<<<<<<<<<<<< День {day + 1} >>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                Console.WriteLine();
                Console.WriteLine("Начало дня:");
                Console.WriteLine();
                Ant[][][] groups = new Ant[colonys.Length][][];

                for (int i = 0; i < colonys.Length; i++) groups[i] = colonys[i].Relever(Heaps);

                for (int i = 0; i < Heaps.Length; i++)
                {
                    Ant[] arrivedes = Heaps[i].Battle(groups, i);
                    foreach (Ant ant in arrivedes)
                    {
                        foreach (Colony colony in colonys)
                        {
                            if (ant.health > 0 && ant != null)
                            {
                                if (ant is Worker && ant.memory == colony.queen.memory)
                                {
                                    Array.Resize(ref colony.workers, colony.queen.workCaunt + 1);
                                    colony.workers[colony.queen.workCaunt] = (Worker)ant;
                                    colony.queen.workCaunt += 1;
                                }
                                if (ant is Warior && ant.memory == colony.queen.memory)
                                {
                                    Array.Resize(ref colony.wariors, colony.queen.warCount + 1);
                                    colony.wariors[colony.queen.warCount] = (Warior)ant;
                                    colony.queen.warCount += 1;
                                }
                                if (ant is Special && ant.memory == colony.queen.memory)
                                {
                                    colony.special = (Special)ant;
                                }
                            }
                            else if (ant is Special && ant.memory == colony.queen.memory)
                            {
                                colony.special.damage = 0;
                                colonys[ant.memory - 1].deads[2] += 1;
                            }
                            else if (ant.health <= 0)
                            {
                                if (ant is Worker && ant.memory == colony.queen.memory)
                                {
                                    colonys[ant.memory - 1].deads[0] += 1;
                                }
                                else if (ant is Warior && ant.memory == colony.queen.memory)
                                {
                                    colonys[ant.memory - 1].deads[1] += 1;
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("Конец дня:");
                Console.WriteLine();

                foreach (Colony colony in colonys)
                {
                    colony.queen.workCaunt = 0;
                    colony.queen.warCount = 0;

                    if (colony.queen.WaitBorn(colony))
                    {
                        Array.Resize(ref colonys, colonys.Length + 1);
                        colonys[^1] = colony.queen.God_save_the_queen(colony.queen, colonys.Length);
                    }

                    Console.WriteLine($"Колония №{colony.queen.memory} {colony.queen.color}:");
                    Console.WriteLine();

                    Console.WriteLine($"{" ",5} рабочие {colony.workers.Length}, войны {colony.wariors.Length }, особый {colony.special.damage != 0}");
                    Console.WriteLine();

                    Console.WriteLine($"{" ",5} добыто ресурсов: веток={ colony.new_resurses[0] }, камней={ colony.new_resurses[1] }, листиков={ colony.new_resurses[2] }, рос={ colony.new_resurses[3] }");
                    Console.WriteLine();

                    Console.WriteLine($"{" ",5} потеряли: { colony.deads[0] } рабочих, { colony.deads[1] } войнов, { colony.deads[2] } специалистов");
                    Console.WriteLine();

                    for (int i = 0; i < colony.new_resurses.Length; i++)
                    {
                        colony.new_resurses[i] = 0;
                        colony.deads[i] = 0;
                    }

                    colony.ResursesInColony();
                    Console.WriteLine();
                }
                for (int i = 0; i < Heaps.Length; i++) Heaps[i].Resurses_in_heap(i);
                Console.WriteLine();

                if (0 < shrew && shrew < 7)
                {
                    Console.WriteLine($"землеройка пробудет еще {shrew} дней!");
                    foreach (Colony colony in colonys)
                    {
                        colony.queen.cicle = 0;
                    }
                    shrew--;
                }
                if (3 < rnd.Next(0, day) && shrew == 7)
                {
                    Console.WriteLine($"О себе решила напомнить землеройка!");
                    foreach (Colony colony in colonys)
                    {
                        colony.queen.cicle = 0;
                    }
                    shrew--;
                }

                static void inf()
                {
                    Console.WriteLine($"Нажмите 'Enter' для того, чтобы перейти в новый день, или 'i' для получения дальнейшей инфломации о колониях");
                    Console.WriteLine($"( 'Enter' / i )");
                    inf1();
                }
                static void inf1()
                {
                    string chois = Console.ReadLine();
                    if ("i" == chois)
                    {
                        Console.WriteLine($"На данный момент живы только {colonys.Length} колонии.О какой из них узнать?");
                        inf2();
                    }
                    else if (chois == "")
                    {
                        Console.Clear();
                    }
                    else
                    {
                        Console.WriteLine("Получен не корректный ответ. Используйте эти кнопки:");
                        Console.WriteLine("-> ( 'Enter' / i ) <-");
                        inf1();
                    }
                }
                static void inf2()
                {
                    int i = Convert.ToInt16(Console.ReadLine()) - 1;

                    if (0 <= i && i <= colonys.Length - 1)
                    {
                        colonys[i].Vois();

                        Console.WriteLine();
                        Console.WriteLine("Чтобы узнать о королеве введи 'q', рабочий - 'work', воин - 'war', специалист - 's'");
                        Console.WriteLine("( q / work / war / s )");
                        inf3(colonys[i]);
                    }
                    else
                    {
                        Console.WriteLine("Получен не корректный ответ. Используйте эти кнопки:");
                        Console.WriteLine($"->( 1 <= ответ <= {colonys.Length} )<-");
                        inf2();
                    }
                }
                static void inf3(Colony colony)
                {
                    Console.WriteLine();
                    switch (Convert.ToString(Console.ReadLine()))
                    {
                        case "q":
                            colony.queen.Vois();
                            Console.WriteLine("Для продолжения нажмите 'Enter'");
                            break;

                        case "work":
                            Console.WriteLine($"Какому именно рабочему из {colony.workers.Length}?");
                            Console.WriteLine();
                            int numWork = Convert.ToInt16(Console.ReadLine());
                            colony.workers[numWork].Vois(colony);
                            Console.WriteLine("Для продолжения нажмите 'Enter'");
                            break;

                        case "war":
                            Console.WriteLine($"Какому именно войну из {colony.wariors.Length}?");
                            Console.WriteLine();
                            int numWar = Convert.ToInt16(Console.ReadLine());
                            colony.wariors[numWar].Vois(colony);
                            Console.WriteLine("Для продолжения нажмите 'Enter'");
                            break;

                        case "s":
                            colony.special.Vois(colony);
                            Console.WriteLine("Для продолжения нажмите 'Enter'");
                            break;

                        default:
                            Console.WriteLine("Получен не корректный ответ. Используйте эти кнопки:");
                            Console.WriteLine("->( q / work / war / s )<-");
                            inf3(colony);
                            break;
                    }
                    inf();
                }
                inf();
            }
            Console.WriteLine($"<<<<<<<<<<<<<<<<<<<<<<<<<<<< Конец >>>>>>>>>>>>>>>>>>>>>>>>>>>>");

            int winnum = 0;
            int winres = 0;
            foreach (var colony in colonys)
            {
                int res = 0;
                for (int i = 0; i < colony.resurses.Length; i++)
                {
                    res += colony.resurses[i];
                }
                if (winres < res)
                {
                    winres = res;
                    winnum = colony.queen.memory;
                }
            }
            Console.WriteLine($"Выжила колония №{winnum} с общими ресурсами {winres}");
        }

        class Ant //Родительский класс, для классов Queen, Warior, Worker и Special
        {
            public string color, clas;
            public int health, protect, damage, memory, target, bite;
            public Ant(string color, int memory, int health, int protect,

                string clas, int damage = 0, int target = 0, int bite = 0)
            {
                this.color = color;
                this.memory = memory;
                this.health = health;
                this.protect = protect;
                this.clas = clas;
                this.damage = damage;
                this.target = target;
                this.bite = bite;
            }
            public void Hit(Ant ant)
            {
                if (this.damage > 0)
                {
                    if (this is Special)
                    {
                        if (((Special)this).mod_doubleDamage)
                        {
                            this.damage *= 2;
                        }
                        if (((Special)this).mod_halfProtect)
                        {
                            this.protect /= 2;
                        }
                    }
                    if (this is Warior)
                    {
                        if (((Warior)this).mod_invulnerability)
                        {
                            this.damage = 1000000000;
                            this.health = 1000000000;
                        }
                        if (ant is Warior && ((Warior)ant).mod_invulnerability)
                        {
                            ant.damage = 1000000000;
                            ant.health = 1000000000;
                        }
                        if (ant is Special && ((Special)ant).mod_protect && ant.health > 0 && ant.damage > 0)
                        {
                            ant.Hit(this);
                        }
                    }
                    if (ant is Worker)
                    {
                        ant.health = 0;
                    }
                    else
                    {
                        for (int j = 0; j < this.bite; j++)
                        {
                            if (ant.protect > 0)
                            {
                                ant.protect -= this.damage;
                                if (ant.protect < 0)
                                {
                                    ant.health += ant.protect;
                                    ant.protect = 0;
                                }
                            }
                            else
                            {
                                ant.health -= this.damage;
                            }
                        }
                    }
                    if (this is Special special)
                    {
                        if (special.mod_doubleDamage)
                        {
                            this.damage /= 2;
                        }
                        if (special.mod_halfProtect)
                        {
                            this.protect *= 2;
                        }
                    }
                    if (this is Warior)
                    {
                        if (((Warior)this).mod_invulnerability)
                        {
                            this.damage = 1;
                            this.health = 1;
                        }
                        if (ant is Warior && ((Warior)ant).mod_invulnerability)
                        {
                            ant.damage = 1;
                            ant.health = 1;
                        }
                    }
                    if (ant.health > 0 && ant.damage > 0)
                    {
                        ant.Hit(this);
                    }
                }
            }
        }

        class Queen : Ant
        {
            Random rnd = new Random();
            public string name;
            public int cicle_First, cicle_Last, queen_First, queen_Last, cicle, queen_count;
            public int workCaunt;
            public int warCount;
            public bool mod_mather;

            public Queen(string color, int memory, int health, int protect, string clas,

                int damage, string name, int cicle_First, int cicle_Last, int queen_First = 0,

                int queen_Last = 0, int workCaunt = 0, int warCount = 0, bool mod_mather = false)

                : base(color, memory, health, protect, clas, damage: damage)
            {
                this.name = name;
                this.cicle_First = cicle_First;
                this.cicle_Last = cicle_Last;
                this.queen_First = queen_First;
                this.queen_Last = queen_Last;
                this.workCaunt = workCaunt;
                this.warCount = warCount;
                this.mod_mather = mod_mather;

                this.cicle = rnd.Next(cicle_First, cicle_Last);
                this.queen_count = rnd.Next(queen_First, queen_Last);
            }

            public bool WaitBorn(Colony colony)
            {
                if (this.cicle == 0)
                {
                    Console.WriteLine($"В Колонии №{colony.queen.memory} рождается новое потомство!");

                    Random rnd = new Random();

                    this.cicle = rnd.Next(cicle_First, cicle_Last);

                    Array.Resize(ref colony.workers, colony.workers.Length + 1);
                    colony.workers[^1] = colony.WorkCall(colony.queen.memory);

                    Array.Resize(ref colony.wariors, colony.wariors.Length + 1);
                    colony.wariors[^1] = colony.MilitaryConscription(colony.queen.memory);

                    if (this.mod_mather == true && this.queen_count > 0)
                    {
                        Console.Write($"В пучине бездны появляется новая жизнь. ");
                        this.queen_count--;

                        if (rnd.Next(1, 10) > 0)
                        {
                            Console.WriteLine($"Та, что приведет новую колонию к величию!");
                            return this.mod_mather;
                        }
                        else
                        {
                            Console.WriteLine($"Но лишь затем, чтобы исчезнуть...");
                            return false;
                        }
                    }
                    else return false;
                }
                else
                {
                    this.cicle--;
                    return false;
                }
            }
            public Colony God_save_the_queen(Queen queen, int memory)
            {
                return new Colony
                (
                    color: queen.color,
                    queen: new Queen(queen.color, memory, queen.health, queen.protect, queen.clas, queen.damage, "Дора", queen.cicle_First, queen.cicle_Last),
                    workers: new Worker[0],
                    wariors: new Warior[0],
                    special: new Special(queen.color, memory, health: 0, protect: 0, damage: 0, clas: "", target: 0, bite: 0)
                );
            }
            public void Vois()
            {
                Console.WriteLine($"Королева {this.name}. Цвет:{this.damage}, здоровье: {this.health}, защиты: {this.protect}, урон:{this.damage}. До нового рождения {this.cicle} дней, королев еще может родить: {this.queen_count}");
                Console.WriteLine();

                Console.WriteLine($"Тип: {this.clas}");
                Console.WriteLine($"Параметры: здоровье = {this.health}, защита = {this.protect}, урон = {this.damage}");
            }
        }

        class Warior : Ant
        {
            public bool mod_off, mod_trap, mod_invulnerability;
            public Warior(string color, int memory, int health, int protect, string clas,

                int damage, int target, int bite, bool mod_off = false,

                bool mod_invulnerability = false, bool mod_trap = false)

                : base(color, memory, health, protect, clas, damage, target, bite)
            {
                this.mod_off = mod_off;
                this.mod_trap = mod_trap;
                this.mod_invulnerability = mod_invulnerability;
            }

            public void Vois(Colony colony)
            {
                Console.WriteLine($"Я служу королеве {colony.queen.name}!. Её цвет:{colony.queen.color}, здоровье: {colony.queen.health}, защита: {colony.queen.protect}, урон:{colony.queen.damage}. До нового рождения {colony.queen.cicle} дней, королев еще может родить: {colony.queen.queen_count}");
                Console.WriteLine();

                Console.WriteLine($"Я Воин. Тип <{this.clas}> из колнии №{this.memory}. Мой цвет:{this.color}, здоровье: {this.health}, защита: {this.protect}, урон:{this.damage}. Могу кусать {this.target} разных врагов {this.bite} раз.");
                Console.ReadLine();
            }
        }

        class Worker : Ant
        {
            static int first = 1;
            static int last = 2;
            public int[] resurses;
            public bool mod_miss, mod_first_protect, mod_plus_res, mod_or;
            public Worker(string color, int memory, int health, int protect, string clas,

                int branch = 0, int stone = 0, int sheet = 0, int dew = 0,

                bool mod_miss = false, bool mod_first_protect = false,

                bool mod_plus_res = false, bool mod_or = false)

                : base(color, memory, health, protect, clas: clas)
            {
                this.resurses = new int[] { branch, stone, sheet, dew };
                this.mod_miss = mod_miss;
                this.mod_first_protect = mod_first_protect;
                this.mod_plus_res = mod_plus_res;
                this.mod_or = mod_or;
            }
            public void Mod_plus_res_on(Heap heap)
            {
                Random rnd = new Random();

                int new_res = rnd.Next(0, 5);

                this.resurses[new_res] += 1;

                GivRes(heap);

                this.resurses[new_res] -= 1;
            }
            public void GivRes(Heap heap)
            {
                foreach (var colony in colonys)
                {
                    if (colony.queen.memory == this.memory)
                    {
                        for (int i = 0; i < heap.resurses.Length; i++)
                        {
                            if (heap.resurses[i] >= this.resurses[i])
                            {
                                if (this.mod_miss && rnd.Next(0, 4) != 1)
                                {
                                    heap.resurses[i] -= this.resurses[i];
                                    colony.resurses[i] += this.resurses[i];
                                    colony.new_resurses[i] += this.resurses[i];
                                }
                                if (this.mod_or)
                                {
                                    if (0 <= this.resurses[i])
                                    {
                                        if (last == 2 && rnd.Next(first, last) == 2)
                                        {
                                            heap.resurses[i] -= this.resurses[i];
                                            colony.resurses[i] += this.resurses[i];
                                            colony.new_resurses[i] += this.resurses[i];
                                            first++;
                                        }
                                        else
                                        {
                                            last--;
                                        }
                                    }

                                }
                                else if (this.mod_miss == false)
                                {
                                    heap.resurses[i] -= this.resurses[i];
                                    colony.resurses[i] += this.resurses[i];
                                    colony.new_resurses[i] += this.resurses[i];
                                }
                            }
                        }
                    }
                }
            }
            public void Vois(Colony colony)
            {
                Console.WriteLine($"Я служу королеве {colony.queen.name}!. Её цвет:{colony.queen.color}, здоровье: {colony.queen.health}, защита: {colony.queen.protect}, урон:{colony.queen.damage}. До нового рождения {colony.queen.cicle} дней, королев еще может родить: {colony.queen.queen_count}");
                Console.WriteLine();

                Console.WriteLine($"Я рабочий. Тип <{this.clas}> из колнии №{this.memory}. Мой цвет:{this.color}, здоровье: {this.health}, защита: {this.protect}. Могу носить: {this.resurses[0]}веточек, {this.resurses[0]}камней, {this.resurses[0]}листиков, {this.resurses[0]}рос.");
                Console.ReadLine();
            }
        }

        class Special : Ant
        {
            public int[] resurses;
            public bool mod_protect, mod_randomTrap, mod_doubleDamage, mod_halfProtect, mod_plusRes;
            public Special(string color, int memory, int health, int protect, string clas, int damage,

                int target, int bite, int branch = 0, int stone = 0, int sheet = 0, int dew = 0,

                bool mod_protect = false, bool mod_randomTrap = false, bool mod_doubleDamage = false,

                bool mod_halfProtect = false, bool mod_plusRes = false)

                : base(color, memory, health, protect, clas, damage, target, bite)
            {
                this.resurses = new int[] { branch, stone, sheet, dew };
                this.mod_protect = mod_protect;
                this.mod_randomTrap = mod_randomTrap;
                this.mod_doubleDamage = mod_doubleDamage;
                this.mod_halfProtect = mod_halfProtect;
                this.mod_plusRes = mod_plusRes;
            }
            public void GivRes(Heap heap)
            {
                foreach (var colony in colonys)
                {
                    if (colony.queen.memory == this.memory)
                    {
                        for (int i = 0; i < heap.resurses.Length; i++)
                        {
                            if (heap.resurses[i] >= this.resurses[i])
                            {
                                heap.resurses[i] -= this.resurses[i];
                                colony.resurses[i] += this.resurses[i];
                                colony.new_resurses[i] += this.resurses[i];
                            }
                        }
                    }
                }
            }
            public void Vois(Colony colony)
            {
                Console.WriteLine($"Я служу королеве {colony.queen.name}!. Её цвет:{colony.queen.color}, здоровье: {colony.queen.health}, защита: {colony.queen.protect}, урон:{colony.queen.damage}. До нового рождения {colony.queen.cicle} дней, королев еще может родить: {colony.queen.queen_count}");
                Console.WriteLine();

                Console.WriteLine($"Я специалист. Тип <{this.clas}> из колнии №{this.memory}. Мой цвет:{this.color}, здоровье: {this.health}, защита: {this.protect}. Могу носить: {this.resurses[0]}веточек, {this.resurses[0]}камней, {this.resurses[0]}листиков, {this.resurses[0]}рос.");
                Console.ReadLine();
            }
        }
        class Colony
        {
            Random rnd = new Random();

            public int[] resurses, new_resurses, deads;
            readonly string color;
            public Queen queen;
            public Worker[] workers;
            public Warior[] wariors;
            public Special special;
            public Colony(string color, Queen queen, Worker[] workers, Warior[] wariors, Special special)
            {
                this.color = color;
                this.queen = queen;
                this.wariors = wariors;
                this.workers = workers;
                this.special = special;
                this.resurses = new int[] { 0, 0, 0, 0 };
                this.new_resurses = new int[] { 0, 0, 0, 0 };
                this.deads = new int[] { 0, 0, 0, 0 };
            }
            public void Vois()
            {
                Console.WriteLine($"Колония «{this.color}» {this.queen.memory}");
                Console.WriteLine($"{" ",5}Королева <{this.queen.name}>: здоровье = {this.queen.health}, защита = {this.queen.protect}, урон = {this.queen.damage}");
                Console.WriteLine($"{" ",5}Ресурсы: к = {this.resurses[0]}, л = {this.resurses[1]}, в = {this.resurses[2]}, р = {this.resurses[3]}");
                Console.WriteLine();

                Console.WriteLine("<<<<<<<<<<<<< Рабочие >>>>>>>>>>>>>");
                Console.WriteLine();
                int[] clases_num_work = new int[] { 0, 0, 0, 0 };

                string[] clases_name_work = new string[] { "элитный", "обычный спринтер", "продвинутый", "элитный забывчивый" };

                string[] parameters_work = new string[] { "здоровье=8, защита=4", "здоровье=1, защита=0", "здоровье=6, защита=2", "здоровье=8, защита=4" };

                for (int i = 0; i < this.workers.Length; i++)
                {
                    for (int j = 0; j < clases_num_work.Length; j++)
                    {
                        if (this.workers[i].clas == clases_name_work[j])
                        {
                            clases_num_work[j]++;
                        }
                    }
                }
                for (int i = 0; i < clases_num_work.Length; i++)
                {
                    Console.WriteLine($"Тип: {clases_name_work[i]}");
                    Console.WriteLine($"{" ",5}Параметры: {parameters_work[i]}");
                    Console.WriteLine($"{" ",5}Количество: {clases_num_work[i]}");
                    Console.WriteLine();

                    Console.Write(clases_name_work[i] == $"обычный спринтер" ? $"{" ",5}Модификатор: не может быть атакован первым" : "");
                    Console.Write(clases_name_work[i] == $"элитный забывчивый" ? $"{" ",5}Модификатор: может забыть взять ресурс из кучи" : "");
                    Console.WriteLine();
                }

                Console.WriteLine("<<<<<<<<<<<<< Воины >>>>>>>>>>>>>");
                Console.WriteLine();

                int[] clases_num_war = new int[] { 0, 0, 0, 0, 0 };

                string[] clases_name_war = new string[] { "старший", "обычный мифический", "элитный", "обычный", "легендарный аномальный" };

                string[] parameters_war = new string[] { "здоровье=2, защита=1, урон=2", "здоровье=1, защита=0, урон=1", "здоровье=8, защита=4, урон=4", "здоровье=1, защита=0, урон=1", "здоровье=10, защита=6, урон=6" };


                for (int i = 0; i < this.wariors.Length; i++)
                {
                    for (int j = 0; j < clases_num_war.Length; j++)
                    {
                        if (this.wariors[i].clas == clases_name_war[j])
                        {
                            clases_num_war[j]++;
                        }
                    }
                }
                for (int i = 0; i < clases_num_war.Length; i++)
                {
                    Console.WriteLine($"Тип: {clases_name_war[i]}");
                    Console.WriteLine($"{" ",5}Параметры: {parameters_war[i]}");
                    Console.WriteLine($"{" ",5}Количество: {clases_num_war[i]}");

                    Console.Write(clases_name_war[i] == "обычный мифический" ? $"{" ",5}Модификатор: полностью неуязвим для всех атак (даже смертельных для неуязвимых), отменяет все модификаторы вражеских воинов, убивает любого с 1 укуса даже неуязвимых." : "");
                    Console.Write(clases_name_war[i] == "легендарный аномальный" ? $"{" ",5}Модификатор: атакует своих вместо врагов." : "");

                    Console.WriteLine();
                }

                Console.WriteLine("<<<<<<<<<<<<< Особые >>>>>>>>>>>>>");
                Console.WriteLine();

                Console.WriteLine($"Тип: {this.special.clas}");

                if (this.special.clas == "ленивый неуязвимый агрессивный неряшливый - Толстоножка")
                {
                    Console.WriteLine($"Параметры: здоровье = 27, защита = 6, урон = 8");

                    Console.WriteLine("Модификаторы:");
                    Console.WriteLine($"{" ",5}не может брать ресурсы; ");
                    Console.WriteLine($"{" ",5}не может быть атакован войнами; ");
                    Console.WriteLine($"{" ",5}защита уменьшена в двое.");
                }
                if (this.special.clas == "трудолюбивый обычный агрессивный дурной бригадир - Толстоножка")
                {
                    Console.WriteLine($"Параметры: здоровье = 21, защита = 9, урон = 9");

                    Console.WriteLine("Модификаторы:");
                    Console.WriteLine($"{" ",5}может брать ресурсы(3 ресурса: веточка);");
                    Console.WriteLine($"{" ",5}может быть атакован войнами;");
                    Console.WriteLine($"{" ",5}атакует врагов(3 цели за раз и наносит 1 укус);");
                    Console.WriteLine($"{" ",5}случайно атакует врагов или своих, наносит двойной урон;");
                    Console.WriteLine($"{" ",5}все рабочие могут брать +1 ресурс.");
                }
                Console.WriteLine("---------------------------------------");
            }
            public void ResursesInColony()
            {
                Console.WriteLine($"В колонии муравьев: №'{this.queen.memory}' : {this.resurses[0]} веток, {this.resurses[1]} камней, {this.resurses[2]} листиков, {this.resurses[3]} рос");
            }
            public Worker WorkCall(int memory)
            {
                Worker[] BlackWorkers_def = new Worker[] {
                    new Worker(color: "Черный", memory: memory, health: 8, protect: 4, clas: "элитный", stone: 1, dew: 1),
                    new Worker(color: "Черный", memory: memory, health: 8, protect: 4, clas: "элитный", stone: 1, sheet: 1),
                    new Worker(color: "Черный", memory: memory, health: 1, protect: 0, clas: "обычный спринтер", sheet: 1, mod_first_protect: true)
                };

                Worker[] GingerWorkers_def = new Worker[] {
                    new Worker(color: "Рыжий", memory: memory, health: 6, protect: 2, clas: "продвинутый", sheet: 2, dew: 2, mod_or: true),
                    new Worker(color: "Рыжий", memory: memory, health: 8, protect: 4, clas: "элитный", stone: 1, dew: 1),
                    new Worker(color: "Рыжий", memory: memory, health: 8, protect: 4, clas: "элитный забывчивый", sheet: 1, dew: 1, mod_miss: true)
                };

                if (this.queen.color == "Рыжий") return GingerWorkers_def[rnd.Next(0, GingerWorkers_def.Length)];

                else if (this.queen.color == "Черный") return BlackWorkers_def[rnd.Next(0, BlackWorkers_def.Length)];

                return null;
            }
            public Warior MilitaryConscription(int memory)
            {
                Warior[] BlackWariors_def = new Warior[] {
                    new Warior(color: "Черный", memory: memory, health: 2, protect: 1, clas: "старший", damage: 2, target: 1, bite: 1),
                    new Warior(color: "Черный", memory: memory, health: 2, protect: 1, clas: "старший", damage: 2, target: 1, bite: 1),
                    new Warior(color: "Черный", memory: memory, health: 1, protect: 0, clas: "обычный мифический", damage: 1, target: 1, bite: 1, mod_invulnerability: true, mod_off: true)
                };

                Warior[] GingerWariors_def = new Warior[] {
                    new Warior(color: "Рыжий", memory: memory, health: 8, protect: 4, clas: "элитный", damage: 4, target: 2, bite: 1),
                    new Warior(color: "Рыжий", memory: memory, health: 1, protect: 0, clas: "обычный", damage: 1, target: 1, bite: 1),
                    new Warior(color: "Рыжий", memory: memory, health: 10, protect: 6,clas: "легендарный аномальный", damage: 6, target: 1, bite: 1, mod_trap: true)
                };
                if (this.queen.color == "Рыжий") return GingerWariors_def[rnd.Next(0, GingerWariors_def.Length)];

                else if (this.queen.color == "Черный") return BlackWariors_def[rnd.Next(0, BlackWariors_def.Length)];

                return null;
            }
            public Ant[][] Relever(Heap[] Heaps)
            {
                Random rnd = new Random();
                Ant[][] groups = new Ant[Heaps.Length][];
                int[] clases = new int[] { 0, 0, 0 };
                for (int i = 0; i < Heaps.Length; i++) groups[i] = new Ant[0];

                foreach (Worker worker in this.workers)
                {
                    int heap = rnd.Next(0, groups.Length);
                    Array.Resize(ref groups[heap], groups[heap].Length + 1);
                    groups[heap][^1] = worker;
                }
                foreach (Warior warior in this.wariors)
                {
                    int heap = rnd.Next(0, groups.Length);
                    Array.Resize(ref groups[heap], groups[heap].Length + 1);
                    groups[heap][^1] = warior;
                }
                if (this.special.damage > 0)
                {
                    int heap = rnd.Next(0, groups.Length);
                    Array.Resize(ref groups[heap], groups[heap].Length + 1);
                    groups[heap][^1] = special;
                }
                for (int i = 0; i < groups.Length; i++)
                {
                    foreach (Ant ant in groups[i])
                    {
                        if (ant is Worker)
                        {
                            clases[0] += 1;
                        }
                        if (ant is Warior)
                        {
                            clases[1] += 1;
                        }
                        if (ant is Special)
                        {
                            clases[2] += 1;
                        }
                    }
                    Console.WriteLine($"С колонии «{this.queen.color}» №{this.queen.memory} в кучу {i} отправилось: {clases[0]} рабочих, {clases[1]} войнов, {clases[2]} особый,");

                    clases[0] = 0;
                    clases[1] = 0;
                    clases[2] = 0;
                }
                Console.WriteLine();

                return groups;
            }
        }
        class Heap
        {
            public int[] resurses;
            public Heap(int branchs, int stones = 0, int sheets = 0, int dews = 0)
            {
                this.resurses = new int[] { branchs, stones, sheets, dews };
            }
            public void Resurses_in_heap(int number)
            {
                Console.WriteLine($"В куче {number++}: {this.resurses[0]} веток, {this.resurses[1]} камней, {this.resurses[2]} листиков, {this.resurses[3]} рос");
            }
            public void Mod_on(Ant[] group)
            {
                foreach (Ant ant in group)
                {
                    if (ant is Special && ant.clas == "ленивый неуязвимый агрессивный неряшливый - Толстоножка")
                    {
                        ((Special)ant).mod_protect = true;
                        ((Special)ant).mod_halfProtect = true;
                    }
                    if (ant is Special && ant.clas == "трудолюбивый обычный агрессивный дурной бригадир - Толстоножка")
                    {
                        ((Special)ant).mod_doubleDamage = true;
                        ((Special)ant).mod_plusRes = true;
                    }
                    if (ant is Worker && ant.clas == "обычный спринтер")
                    {
                        ((Worker)ant).mod_first_protect = true;
                    }
                    if (ant is Worker && ant.clas == "элитный забывчивый")
                    {
                        ((Worker)ant).mod_miss = true;
                    }
                    if (ant is Warior && ant.clas == "обычный мифический")
                    {
                        ((Warior)ant).mod_off = true;
                    }
                    if (ant is Warior && ant.clas == "легендарный аномальный")
                    {
                        ((Warior)ant).mod_trap = true;
                    }
                }
            }
            public void Mod_off(Ant[] group)
            {
                foreach (Ant ant in group)
                {
                    if (ant is Warior)
                    {
                        if (ant.clas == "обычный мифический")
                        {
                            ((Warior)ant).mod_off = false;
                        }
                        if (ant.clas == "легендарный аномальный")
                        {
                            ((Warior)ant).mod_trap = false;
                        }
                    }
                }
            }
            public void FindOpponent(int num_heap, Ant ant, Ant[][] group1, Ant[][] group2)
            {
                Ant opponent = group2[num_heap][rnd.Next(0, group2[num_heap].Length)];

                Ant ally = group1[num_heap][rnd.Next(0, group1[num_heap].Length)];

                if (opponent.health < 0 || ally.health < 0)
                {
                    if (opponent is Worker && ((Worker)opponent).mod_first_protect)
                    {
                        ((Worker)opponent).mod_first_protect = false;
                        FindOpponent(num_heap, ant, group1, group2);
                    }
                    else if (ant is Special && ((Special)ant).mod_randomTrap && rnd.Next(0, 2) == 1 || ant is Warior && ((Warior)ant).mod_trap && ant != ally)
                    {
                        ant.Hit(ally);
                    }
                    else
                    {
                        ant.Hit(opponent);
                    }
                }
                else
                {
                    ant.Hit(opponent);
                }


            }

            public Ant[] Battle(Ant[][][] groups, int num_heap)
            {
                foreach (Ant[][] group1 in groups)
                {
                    foreach (Ant[][] group2 in groups)
                    {
                        if (group1 != group2)
                        {
                            foreach (Ant ant in group1[num_heap])
                            {
                                if (ant is Warior && ((Warior)ant).mod_off == true)
                                {
                                    this.Mod_off(group2[num_heap]);
                                    break;
                                }
                            }
                        }
                    }
                }
                foreach (Ant[][] group in groups)
                {
                    foreach (Ant ant in group[num_heap])
                    {
                        if (ant is Special && ((Special)ant).mod_plusRes)
                        {
                            foreach (Ant again_ant in group[num_heap])
                            {
                                if (again_ant is Worker)
                                {
                                    ((Worker)again_ant).mod_plus_res = true;
                                }
                            }
                            break;
                        }
                    }
                }
                foreach (Ant[][] group1 in groups)
                {
                    foreach (Ant[][] group2 in groups)
                    {
                        if (group1[num_heap].Length != 0 && group2[num_heap].Length != 0 && group1 != group2 && group1[num_heap][0].color != group2[num_heap][0].color)
                        {
                            foreach (Ant ant in group1[num_heap])
                            {
                                for (int k = 0; k < ant.target; k++)
                                {
                                    FindOpponent(num_heap, ant, group1, group2);
                                }
                            }
                        }
                    }
                }
                int count = 0;
                foreach (Ant[][] group in groups) count += group[num_heap].Length;

                Ant[] returners = new Ant[count];
                count = 0;

                foreach (Ant[][] group1 in groups)
                {
                    for (int i = 0; i < group1[num_heap].Length; i++)
                    {
                        returners[i + count] = group1[num_heap][i];
                    }
                    count += group1[num_heap].Length;
                }

                foreach (Ant[][] group in groups)
                {
                    foreach (Ant ant in group[num_heap])
                    {
                        if (ant is Worker worker && worker.mod_plus_res)
                        {
                            worker.mod_plus_res = false;
                        }
                    }
                    this.Mod_on(group[num_heap]);
                }

                foreach (Ant ant in returners)
                {
                    if (ant is Worker && ant.health > 0)
                    {
                        if (((Worker)ant).mod_plus_res == true)
                        {
                            ((Worker)ant).Mod_plus_res_on(this);
                        }
                        else
                        {
                            ((Worker)ant).GivRes(this);
                        }
                    }
                    if (ant is Special && ant.health > 0)
                    {
                        ((Special)ant).GivRes(this);
                    }
                }

                return returners;
            }
        }
    }
}