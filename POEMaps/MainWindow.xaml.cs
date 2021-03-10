using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using POEMaps;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace POEMaps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public static class Regions
    {
        public static string tirns_end { get { return "Tirn's End"; } }
        public static string glennach_cairns { get { return "Glennach Cairns"; } }
        public static string lex_proxima { get { return "Lex Proxima"; } }
        public static string lex_ejoris { get { return "Lex Ejoris"; } }
        public static string lira_arthain { get { return "Lira Arthain"; } }
        public static string valdos_rest { get { return "Valdo's Rest"; } }
        public static string new_vastir { get { return "New Vastir"; } }
        public static string haewark_hamlet { get { return "Haewark Hamlet"; } }
    }


    public partial class MainWindow : Window
    {

        List<Map> maps = new List<Map>();
        List<String> selected_zones = new List<String>();
        List<Map> filteredMaps = new List<Map>();
        int selected_watchstones = -1;
        Dictionary<int, bool> tiers_shown; //tier, selected
        List<Map> selected_maps = new List<Map>();
        RequestResult request;
        bool searchTaskRunning = false;


        

        public void logInfo(string s)
        {
            TextBlock t = new TextBlock();
            t.Text = s;
            t.FontSize = 14;
            logsList.Children.Add(t);
        }
        

        public void logError(string s)
        {
            TextBlock t = new TextBlock();
            t.Text = s;
            t.FontSize = 14;
            t.Foreground = Brushes.Red;
            logsList.Children.Add(t);
        }

        public void InitializeDictionary()
        {
            tiers_shown = new Dictionary<int, bool>();
            for(int i = 1; i <= 16; i++)
                tiers_shown.Add(i, false);
        }

        public void InitializeMaps()
        {
            // tier 1
            maps.Add(new Map("Arcade Map", "arcade-map-tier-1", "Glennach Cairns", 1));
            maps.Add(new Map("Coves Map", "coves-map-tier-1", "Tirn's End", 1));
            maps.Add(new Map("Jungle Valley Map", "jungle-valley-map-tier-1", "Lex Proxima", 1));
            maps.Add(new Map("Pen Map", "pen-map-tier-1", "Valdo's Rest", 1));

            // tier 2
            maps.Add(new Map("Arena Map", "arena-map-tier-2", "Valdo's Rest", 2));
            maps.Add(new Map("Crater Map", "crater-map-tier-2", "Lex Proxima", 2));
            maps.Add(new Map("Defiled Cathedral Map", "defiled-cathedral-map-tier-2", "Lex Proxima", 2));
            maps.Add(new Map("Fields Map", "fields-map-tier-2", "Tirn's End", 2));
            maps.Add(new Map("Frozen Cabins Map", "frozen-cabins-map-tier-2", "Lex Proxima", 2));
            maps.Add(new Map("Gardens Map", "gardens-map-tier-2", "Valdo's Rest", 2));
            maps.Add(new Map("Grotto Map", "grotto-map-tier-2", "Glennach Cairns", 2));
            maps.Add(new Map("Infested Valley Map", "infested-valley-map-tier-2", "Glennach Cairns", 2));
            maps.Add(new Map("Lair Map", "lair-map-tier-2", "Valdo's Rest", 2));
            maps.Add(new Map("Moon Temple Map", "moon-temple-map-tier-2", "Tirn's End", 2));
            maps.Add(new Map("Peninsula Map", "peninsula-map-tier-2", "Lex Proxima", 2));
            maps.Add(new Map("Promenade Map", "promenade-map-tier-2", "Valdo's Rest", 2));
            maps.Add(new Map("Spider Forest Map", "spider-forest-map-tier-2", "Lex Proxima", 2));
            maps.Add(new Map("Sunken City Map", "sunken-city-map-tier-2", "Glennach Cairns", 2));
            maps.Add(new Map("Tower Map", "tower-map-tier-2", "Glennach Cairns", 2));
            maps.Add(new Map("Tropical Island Map", "tropical-island-map-tier-2", "Glennach Cairns", 2));
            maps.Add(new Map("Underground River Map", "underground-river-map-tier-2", "Tirn's End", 2));
            maps.Add(new Map("Underground Sea Map", "underground-sea-map-tier-2", "Lex Proxima", 2));
            maps.Add(new Map("Volcano Map", "volcano-map-tier-2", "Lex Proxima", 2));
            maps.Add(new Map("Wharf Map", "wharf-map-tier-2", "Tirn's End", 2));

            // tier 3
            maps.Add(new Map("Ancient City Map", "ancient-city-map-tier-3", "Glennach Cairns", 3));
            maps.Add(new Map("Arid Lake Map", "arid-lake-map-tier-3", "Valdo's Rest", 3));
            maps.Add(new Map("Ashen Wood Map", "ashen-wood-map-tier-3", "Valdo's Rest", 3));
            maps.Add(new Map("Basilica Map", "basilica-map-tier-3", "Glennach Cairns", 3));
            maps.Add(new Map("Belfry Map", "belfry-map-tier-3", "New Vastir", 3));
            maps.Add(new Map("Canyon Map", "canyon-map-tier-3", "Valdo's Rest", 3));
            maps.Add(new Map("Cemetery Map", "cemetery-map-tier-3", "Haewark Hamlet", 3));
            maps.Add(new Map("Courtyard Map", "courtyard-map-tier-3", "Lex Proxima", 3));
            maps.Add(new Map("Factory Map", "factory-map-tier-3", "Tirn's End", 3));
            maps.Add(new Map("Forking River Map", "forking-river-map-tier-3", "Lex Ejoris", 3));
            maps.Add(new Map("Geode Map", "geode-map-tier-3", "Lex Proxima", 3));
            maps.Add(new Map("Glacier Map", "glacier-map-tier-3", "Lex Proxima", 3));
            maps.Add(new Map("Graveyard Map", "graveyard-map-tier-3", "Glennach Cairns", 3));
            maps.Add(new Map("Laboratory Map", "laboratory-map-tier-3", "New Vastir", 3));
            maps.Add(new Map("Lava Chamber Map", "lava-chamber-map-tier-3", "Glennach Cairns", 3));
            maps.Add(new Map("Mud Geyser Map", "mud-geyser-map-tier-3", "Tirn's End", 3));
            maps.Add(new Map("Orchard Map", "orchard-map-tier-3", "Valdo's Rest", 3));
            maps.Add(new Map("Overgrown Ruin Map", "overgrown-ruin-map-tier-3", "Lex Proxima", 3));
            maps.Add(new Map("Pier Map", "pier-map-tier-3", "Lira Arthain", 3));
            maps.Add(new Map("Plaza Map", "plaza-map-tier-3", "Haewark Hamlet", 3));
            maps.Add(new Map("Primordial Blocks Map", "primordial-blocks-map-tier-3", "Lira Arthain", 3));
            maps.Add(new Map("Reef Map", "reef-map-tier-3", "Lex Proxima", 3));
            maps.Add(new Map("Shore Map", "shore-map-tier-3", "Tirn's End", 3));
            maps.Add(new Map("Strand Map", "strand-map-tier-3", "New Vastir", 3));
            maps.Add(new Map("Sulphur Vents Map", "sulphur-vents-map-tier-3", "Lex Proxima", 3));
            maps.Add(new Map("Temple Map", "temple-map-tier-3", "Tirn's End", 3));
            maps.Add(new Map("Toxic Sewer Map", "toxic-sewer-map-tier-3", "Lira Arthain", 3));
            maps.Add(new Map("Vaal Pyramid Map", "vaal-pyramid-map-tier-3", "Lex Proxima", 3));

            // tier 4
            maps.Add(new Map("Academy Map", "academy-map-tier-4", "Lex Proxima", 4));
            maps.Add(new Map("Arcade Map", "arcade-map-tier-4", "Glennach Cairns", 4));
            maps.Add(new Map("Arsenal Map", "arsenal-map-tier-4", "Lex Ejoris", 4));
            maps.Add(new Map("Bone Crypt Map", "bone-crypt-map-tier-4", "Lex Ejoris", 4));
            maps.Add(new Map("Cage Map", "cage-map-tier-4", "Glennach Cairns", 4));
            maps.Add(new Map("Channel Map", "channel-map-tier-4", "Haewark Hamlet", 4));
            maps.Add(new Map("Courthouse Map", "courthouse-map-tier-4", "Lira Arthain", 4));
            maps.Add(new Map("Coves Map", "coves-map-tier-4", "Tirn's End", 4));
            maps.Add(new Map("Crimson Temple Map", "crimson-temple-map-tier-4", "New Vastir", 4));
            maps.Add(new Map("Crystal Ore Map", "crystal-ore-map-tier-4", "Lex Ejoris", 4));
            maps.Add(new Map("Desert Map", "desert-map-tier-4", "New Vastir", 4));
            maps.Add(new Map("Dunes Map", "dunes-map-tier-4", "New Vastir", 4));
            maps.Add(new Map("Iceberg Map", "iceberg-map-tier-4", "Glennach Cairns", 4));
            maps.Add(new Map("Jungle Valley Map", "jungle-valley-map-tier-4", "Lex Proxima", 4));
            maps.Add(new Map("Leyline Map", "leyline-map-tier-4", "Haewark Hamlet", 4));
            maps.Add(new Map("Lookout Map", "lookout-map-tier-4", "Lira Arthain", 4));
            maps.Add(new Map("Marshes Map", "marshes-map-tier-4", "Lira Arthain", 4));
            maps.Add(new Map("Museum Map", "museum-map-tier-4", "Glennach Cairns", 4));
            maps.Add(new Map("Pen Map", "pen-map-tier-4", "Valdo's Rest", 4));
            maps.Add(new Map("Precinct Map", "precinct-map-tier-4", "Lira Arthain", 4));
            maps.Add(new Map("Primordial Pool Map", "primordial-pool-map-tier-4", "Lex Ejoris", 4));
            maps.Add(new Map("Ramparts Map", "ramparts-map-tier-4", "Haewark Hamlet", 4));
            maps.Add(new Map("Wasteland Map", "wasteland-map-tier-4", "Glennach Cairns", 4));

            // tier 5
            maps.Add(new Map("Armoury Map", "armoury-map-tier-5", "Lex Ejoris", 5));
            maps.Add(new Map("Atoll Map", "atoll-map-tier-5", "Haewark Hamlet", 5));
            maps.Add(new Map("Carcass Map", "carcass-map-tier-5", "Lex Proxima", 5));
            maps.Add(new Map("Cells Map", "cells-map-tier-5", "Lex Ejoris", 5));
            maps.Add(new Map("Chateau Map", "chateau-map-tier-5", "Tirn's End", 5));
            maps.Add(new Map("Crater Map", "crater-map-tier-5", "Lex Proxima", 5));
            maps.Add(new Map("Desert Spring Map", "desert-spring-map-tier-5", "Tirn's End", 5));
            maps.Add(new Map("Fields Map", "fields-map-tier-5", "Tirn's End", 5));
            maps.Add(new Map("Grotto Map", "grotto-map-tier-5", "Glennach Cairns", 5));
            maps.Add(new Map("Haunted Mansion Map", "haunted-mansion-map-tier-5", "Glennach Cairns", 5));
            maps.Add(new Map("Infested Valley Map", "infested-valley-map-tier-5", "Glennach Cairns", 5));
            maps.Add(new Map("Lair Map", "lair-map-tier-5", "Valdo's Rest", 5));
            maps.Add(new Map("Lighthouse Map", "lighthouse-map-tier-5", "Lira Arthain", 5));
            maps.Add(new Map("Moon Temple Map", "moon-temple-map-tier-5", "Tirn's End", 5));
            maps.Add(new Map("Palace Map", "palace-map-tier-5", "Haewark Hamlet", 5));
            maps.Add(new Map("Peninsula Map", "peninsula-map-tier-5", "Lex Proxima", 5));
            maps.Add(new Map("Promenade Map", "promenade-map-tier-5", "Valdo's Rest", 5));
            maps.Add(new Map("Shrine Map", "shrine-map-tier-5", "New Vastir", 5));
            maps.Add(new Map("Tower Map", "tower-map-tier-5", "Glennach Cairns", 5));
            maps.Add(new Map("Waste Pool Map", "waste-pool-map-tier-5", "Lex Ejoris", 5));

            // tier 6
            maps.Add(new Map("Ancient City Map", "ancient-city-map-tier-6", "Glennach Cairns", 6));
            maps.Add(new Map("Arena Map", "arena-map-tier-6", "Valdo's Rest", 6));
            maps.Add(new Map("Arid Lake Map", "arid-lake-map-tier-6", "Valdo's Rest", 6));
            maps.Add(new Map("Basilica Map", "basilica-map-tier-6", "Glennach Cairns", 6));
            maps.Add(new Map("Belfry Map", "belfry-map-tier-6", "New Vastir", 6));
            maps.Add(new Map("Defiled Cathedral Map", "defiled-cathedral-map-tier-6", "Lex Proxima", 6));
            maps.Add(new Map("Forking River Map", "forking-river-map-tier-6", "Lex Ejoris", 6));
            maps.Add(new Map("Frozen Cabins Map", "frozen-cabins-map-tier-6", "Lex Proxima", 6));
            maps.Add(new Map("Gardens Map", "gardens-map-tier-6", "Valdo's Rest", 6));
            maps.Add(new Map("Glacier Map", "glacier-map-tier-6", "Lex Proxima", 6));
            maps.Add(new Map("Lava Chamber Map", "lava-chamber-map-tier-6", "Glennach Cairns", 6));
            maps.Add(new Map("Malformation Map", "malformation-map-tier-6", "Glennach Cairns", 6));
            maps.Add(new Map("Mud Geyser Map", "mud-geyser-map-tier-6", "Tirn's End", 6));
            maps.Add(new Map("Orchard Map", "orchard-map-tier-6", "Valdo's Rest", 6));
            maps.Add(new Map("Plaza Map", "plaza-map-tier-6", "Haewark Hamlet", 6));
            maps.Add(new Map("Primordial Blocks Map", "primordial-blocks-map-tier-6", "Lira Arthain", 6));
            maps.Add(new Map("Reef Map", "reef-map-tier-6", "Lex Proxima", 6));
            maps.Add(new Map("Shore Map", "shore-map-tier-6", "Tirn's End", 6));
            maps.Add(new Map("Silo Map", "silo-map-tier-6", "Tirn's End", 6));
            maps.Add(new Map("Spider Forest Map", "spider-forest-map-tier-6", "Lex Proxima", 6));
            maps.Add(new Map("Sulphur Vents Map", "sulphur-vents-map-tier-6", "Lex Proxima", 6));
            maps.Add(new Map("Sunken City Map", "sunken-city-map-tier-6", "Glennach Cairns", 6));
            maps.Add(new Map("Temple Map", "temple-map-tier-6", "Tirn's End", 6));
            maps.Add(new Map("Tropical Island Map", "tropical-island-map-tier-6", "Glennach Cairns", 6));
            maps.Add(new Map("Underground River Map", "underground-river-map-tier-6", "Tirn's End", 6));
            maps.Add(new Map("Underground Sea Map", "underground-sea-map-tier-6", "Lex Proxima", 6));
            maps.Add(new Map("Volcano Map", "volcano-map-tier-6", "Lex Proxima", 6));
            maps.Add(new Map("Wharf Map", "wharf-map-tier-6", "Tirn's End", 6));

            // tier 7
            maps.Add(new Map("Academy Map", "academy-map-tier-7", "Lex Proxima", 7));
            maps.Add(new Map("Arsenal Map", "arsenal-map-tier-7", "Lex Ejoris", 7));
            maps.Add(new Map("Ashen Wood Map", "ashen-wood-map-tier-7", "Valdo's Rest", 7));
            maps.Add(new Map("Bone Crypt Map", "bone-crypt-map-tier-7", "Lex Ejoris", 7));
            maps.Add(new Map("Cage Map", "cage-map-tier-7", "Glennach Cairns", 7));
            maps.Add(new Map("Canyon Map", "canyon-map-tier-7", "Valdo's Rest", 7));
            maps.Add(new Map("Cemetery Map", "cemetery-map-tier-7", "Haewark Hamlet", 7));
            maps.Add(new Map("Channel Map", "channel-map-tier-7", "Haewark Hamlet", 7));
            maps.Add(new Map("Courtyard Map", "courtyard-map-tier-7", "Lex Proxima", 7));
            maps.Add(new Map("Crystal Ore Map", "crystal-ore-map-tier-7", "Lex Ejoris", 7));
            maps.Add(new Map("Dark Forest Map", "dark-forest-map-tier-7", "Valdo's Rest", 7));
            maps.Add(new Map("Factory Map", "factory-map-tier-7", "Tirn's End", 7));
            maps.Add(new Map("Geode Map", "geode-map-tier-7", "Lex Proxima", 7));
            maps.Add(new Map("Graveyard Map", "graveyard-map-tier-7", "Glennach Cairns", 7));
            maps.Add(new Map("Laboratory Map", "laboratory-map-tier-7", "New Vastir", 7));
            maps.Add(new Map("Leyline Map", "leyline-map-tier-7", "Haewark Hamlet", 7));
            maps.Add(new Map("Lookout Map", "lookout-map-tier-7", "Lira Arthain", 7));
            maps.Add(new Map("Marshes Map", "marshes-map-tier-7", "Lira Arthain", 7));
            maps.Add(new Map("Museum Map", "museum-map-tier-7", "Glennach Cairns", 7));
            maps.Add(new Map("Overgrown Ruin Map", "overgrown-ruin-map-tier-7", "Lex Proxima", 7));
            maps.Add(new Map("Pier Map", "pier-map-tier-7", "Lira Arthain", 7));
            maps.Add(new Map("Primordial Pool Map", "primordial-pool-map-tier-7", "Lex Ejoris", 7));
            maps.Add(new Map("Ramparts Map", "ramparts-map-tier-7", "Haewark Hamlet", 7));
            maps.Add(new Map("Strand Map", "strand-map-tier-7", "New Vastir", 7));
            maps.Add(new Map("Toxic Sewer Map", "toxic-sewer-map-tier-7", "Lira Arthain", 7));
            maps.Add(new Map("Vaal Pyramid Map", "vaal-pyramid-map-tier-7", "Lex Proxima", 7));
            maps.Add(new Map("Waterways Map", "waterways-map-tier-7", "Haewark Hamlet", 7));

            // tier 8
            maps.Add(new Map("Alleyways Map", "alleyways-map-tier-8", "Lex Ejoris", 8));
            maps.Add(new Map("Arcade Map", "arcade-map-tier-8", "Glennach Cairns", 8));
            maps.Add(new Map("Armoury Map", "armoury-map-tier-8", "Lex Ejoris", 8));
            maps.Add(new Map("Cells Map", "cells-map-tier-8", "Lex Ejoris", 8));
            maps.Add(new Map("Courthouse Map", "courthouse-map-tier-8", "Lira Arthain", 8));
            maps.Add(new Map("Coves Map", "coves-map-tier-8", "Tirn's End", 8));
            maps.Add(new Map("Crimson Temple Map", "crimson-temple-map-tier-8", "New Vastir", 8));
            maps.Add(new Map("Desert Map", "desert-map-tier-8", "New Vastir", 8));
            maps.Add(new Map("Dry Sea Map", "dry-sea-map-tier-8", "Valdo's Rest", 8));
            maps.Add(new Map("Dunes Map", "dunes-map-tier-8", "New Vastir", 8));
            maps.Add(new Map("Iceberg Map", "iceberg-map-tier-8", "Glennach Cairns", 8));
            maps.Add(new Map("Jungle Valley Map", "jungle-valley-map-tier-8", "Lex Proxima", 8));
            maps.Add(new Map("Palace Map", "palace-map-tier-8", "Haewark Hamlet", 8));
            maps.Add(new Map("Pen Map", "pen-map-tier-8", "Valdo's Rest", 8));
            maps.Add(new Map("Precinct Map", "precinct-map-tier-8", "Lira Arthain", 8));
            maps.Add(new Map("Racecourse Map", "racecourse-map-tier-8", "Lira Arthain", 8));
            maps.Add(new Map("Waste Pool Map", "waste-pool-map-tier-8", "Lex Ejoris", 8));
            maps.Add(new Map("Wasteland Map", "wasteland-map-tier-8", "Glennach Cairns", 8));

            // tier 9
            maps.Add(new Map("Arena Map", "arena-map-tier-9", "Valdo's Rest", 9));
            maps.Add(new Map("Atoll Map", "atoll-map-tier-9", "Haewark Hamlet", 9));
            maps.Add(new Map("Basilica Map", "basilica-map-tier-9", "Glennach Cairns", 9));
            maps.Add(new Map("Belfry Map", "belfry-map-tier-9", "New Vastir", 9));
            maps.Add(new Map("Carcass Map", "carcass-map-tier-9", "Lex Proxima", 9));
            maps.Add(new Map("Chateau Map", "chateau-map-tier-9", "Tirn's End", 9));
            maps.Add(new Map("Crater Map", "crater-map-tier-9", "Lex Proxima", 9));
            maps.Add(new Map("Dark Forest Map", "dark-forest-map-tier-9", "Valdo's Rest", 9));
            maps.Add(new Map("Defiled Cathedral Map", "defiled-cathedral-map-tier-9", "Lex Proxima", 9));
            maps.Add(new Map("Desert Spring Map", "desert-spring-map-tier-9", "Tirn's End", 9));
            maps.Add(new Map("Dungeon Map", "dungeon-map-tier-9", "Lex Ejoris", 9));
            maps.Add(new Map("Fields Map", "fields-map-tier-9", "Tirn's End", 9));
            maps.Add(new Map("Gardens Map", "gardens-map-tier-9", "Valdo's Rest", 9));
            maps.Add(new Map("Grotto Map", "grotto-map-tier-9", "Glennach Cairns", 9));
            maps.Add(new Map("Haunted Mansion Map", "haunted-mansion-map-tier-9", "Glennach Cairns", 9));
            maps.Add(new Map("Infested Valley Map", "infested-valley-map-tier-9", "Glennach Cairns", 9));
            maps.Add(new Map("Lair Map", "lair-map-tier-9", "Valdo's Rest", 9));
            maps.Add(new Map("Lighthouse Map", "lighthouse-map-tier-9", "Lira Arthain", 9));
            maps.Add(new Map("Mausoleum Map", "mausoleum-map-tier-9", "Lira Arthain", 9));
            maps.Add(new Map("Mineral Pools Map", "mineral-pools-map-tier-9", "Valdo's Rest", 9));
            maps.Add(new Map("Moon Temple Map", "moon-temple-map-tier-9", "Tirn's End", 9));
            maps.Add(new Map("Orchard Map", "orchard-map-tier-9", "Valdo's Rest", 9));
            maps.Add(new Map("Overgrown Shrine Map", "overgrown-shrine-map-tier-9", "Lex Proxima", 9));
            maps.Add(new Map("Peninsula Map", "peninsula-map-tier-9", "Lex Proxima", 9));
            maps.Add(new Map("Promenade Map", "promenade-map-tier-9", "Valdo's Rest", 9));
            maps.Add(new Map("Relic Chambers Map", "relic-chambers-map-tier-9", "New Vastir", 9));
            maps.Add(new Map("Shrine Map", "shrine-map-tier-9", "New Vastir", 9));
            maps.Add(new Map("Silo Map", "silo-map-tier-9", "Tirn's End", 9));
            maps.Add(new Map("Spider Forest Map", "spider-forest-map-tier-9", "Lex Proxima", 9));
            maps.Add(new Map("Spider Lair Map", "spider-lair-map-tier-9", "Glennach Cairns", 9));
            maps.Add(new Map("Stagnation Map", "stagnation-map-tier-9", "Glennach Cairns", 9));
            maps.Add(new Map("Sunken City Map", "sunken-city-map-tier-9", "Glennach Cairns", 9));
            maps.Add(new Map("Tower Map", "tower-map-tier-9", "Glennach Cairns", 9));
            maps.Add(new Map("Tropical Island Map", "tropical-island-map-tier-9", "Glennach Cairns", 9));
            maps.Add(new Map("Underground River Map", "underground-river-map-tier-9", "Tirn's End", 9));
            maps.Add(new Map("Underground Sea Map", "underground-sea-map-tier-9", "Lex Proxima", 9));
            maps.Add(new Map("Volcano Map", "volcano-map-tier-9", "Lex Proxima", 9));
            maps.Add(new Map("Wharf Map", "wharf-map-tier-9", "Tirn's End", 9));

            // tier 10
            maps.Add(new Map("Ancient City Map", "ancient-city-map-tier-10", "Glennach Cairns", 10));
            maps.Add(new Map("Arcade Map", "arcade-map-tier-10", "Glennach Cairns", 10));
            maps.Add(new Map("Arid Lake Map", "arid-lake-map-tier-10", "Valdo's Rest", 10));
            maps.Add(new Map("Ashen Wood Map", "ashen-wood-map-tier-10", "Valdo's Rest", 10));
            maps.Add(new Map("Canyon Map", "canyon-map-tier-10", "Valdo's Rest", 10));
            maps.Add(new Map("Cemetery Map", "cemetery-map-tier-10", "Haewark Hamlet", 10));
            maps.Add(new Map("Courtyard Map", "courtyard-map-tier-10", "Lex Proxima", 10));
            maps.Add(new Map("Coves Map", "coves-map-tier-10", "Tirn's End", 10));
            maps.Add(new Map("Crystal Ore Map", "crystal-ore-map-tier-10", "Lex Ejoris", 10));
            maps.Add(new Map("Dry Sea Map", "dry-sea-map-tier-10", "Valdo's Rest", 10));
            maps.Add(new Map("Factory Map", "factory-map-tier-10", "Tirn's End", 10));
            maps.Add(new Map("Forbidden Woods Map", "forbidden-woods-map-tier-10", "Lira Arthain", 10));
            maps.Add(new Map("Forking River Map", "forking-river-map-tier-10", "Lex Ejoris", 10));
            maps.Add(new Map("Frozen Cabins Map", "frozen-cabins-map-tier-10", "Lex Proxima", 10));
            maps.Add(new Map("Geode Map", "geode-map-tier-10", "Lex Proxima", 10));
            maps.Add(new Map("Ghetto Map", "ghetto-map-tier-10", "Valdo's Rest", 10));
            maps.Add(new Map("Glacier Map", "glacier-map-tier-10", "Lex Proxima", 10));
            maps.Add(new Map("Graveyard Map", "graveyard-map-tier-10", "Glennach Cairns", 10));
            maps.Add(new Map("Jungle Valley Map", "jungle-valley-map-tier-10", "Lex Proxima", 10));
            maps.Add(new Map("Laboratory Map", "laboratory-map-tier-10", "New Vastir", 10));
            maps.Add(new Map("Lava Chamber Map", "lava-chamber-map-tier-10", "Glennach Cairns", 10));
            maps.Add(new Map("Leyline Map", "leyline-map-tier-10", "Haewark Hamlet", 10));
            maps.Add(new Map("Lookout Map", "lookout-map-tier-10", "Lira Arthain", 10));
            maps.Add(new Map("Malformation Map", "malformation-map-tier-10", "Glennach Cairns", 10));
            maps.Add(new Map("Marshes Map", "marshes-map-tier-10", "Lira Arthain", 10));
            maps.Add(new Map("Mud Geyser Map", "mud-geyser-map-tier-10", "Tirn's End", 10));
            maps.Add(new Map("Overgrown Ruin Map", "overgrown-ruin-map-tier-10", "Lex Proxima", 10));
            maps.Add(new Map("Pen Map", "pen-map-tier-10", "Valdo's Rest", 10));
            maps.Add(new Map("Phantasmagoria Map", "phantasmagoria-map-tier-10", "Lex Proxima", 10));
            maps.Add(new Map("Pier Map", "pier-map-tier-10", "Lira Arthain", 10));
            maps.Add(new Map("Plaza Map", "plaza-map-tier-10", "Haewark Hamlet", 10));
            maps.Add(new Map("Primordial Blocks Map", "primordial-blocks-map-tier-10", "Lira Arthain", 10));
            maps.Add(new Map("Primordial Pool Map", "primordial-pool-map-tier-10", "Lex Ejoris", 10));
            maps.Add(new Map("Reef Map", "reef-map-tier-10", "Lex Proxima", 10));
            maps.Add(new Map("Scriptorium Map", "scriptorium-map-tier-10", "Glennach Cairns", 10));
            maps.Add(new Map("Shore Map", "shore-map-tier-10", "Tirn's End", 10));
            maps.Add(new Map("Strand Map", "strand-map-tier-10", "New Vastir", 10));
            maps.Add(new Map("Sulphur Vents Map", "sulphur-vents-map-tier-10", "Lex Proxima", 10));
            maps.Add(new Map("Temple Map", "temple-map-tier-10", "Tirn's End", 10));
            maps.Add(new Map("Toxic Sewer Map", "toxic-sewer-map-tier-10", "Lira Arthain", 10));
            maps.Add(new Map("Vaal Pyramid Map", "vaal-pyramid-map-tier-10", "Lex Proxima", 10));

            // tier 11
            maps.Add(new Map("Academy Map", "academy-map-tier-11", "Lex Proxima", 11));
            maps.Add(new Map("Ancient City Map", "ancient-city-map-tier-11", "Glennach Cairns", 11));
            maps.Add(new Map("Arena Map", "arena-map-tier-11", "Valdo's Rest", 11));
            maps.Add(new Map("Arid Lake Map", "arid-lake-map-tier-11", "Valdo's Rest", 11));
            maps.Add(new Map("Armoury Map", "armoury-map-tier-11", "Lex Ejoris", 11));
            maps.Add(new Map("Arsenal Map", "arsenal-map-tier-11", "Lex Ejoris", 11));
            maps.Add(new Map("Ashen Wood Map", "ashen-wood-map-tier-11", "Valdo's Rest", 11));
            maps.Add(new Map("Belfry Map", "belfry-map-tier-11", "New Vastir", 11));
            maps.Add(new Map("Bone Crypt Map", "bone-crypt-map-tier-11", "Lex Ejoris", 11));
            maps.Add(new Map("Cage Map", "cage-map-tier-11", "Glennach Cairns", 11));
            maps.Add(new Map("Carcass Map", "carcass-map-tier-11", "Lex Proxima", 11));
            maps.Add(new Map("Cells Map", "cells-map-tier-11", "Lex Ejoris", 11));
            maps.Add(new Map("Channel Map", "channel-map-tier-11", "Haewark Hamlet", 11));
            maps.Add(new Map("Chateau Map", "chateau-map-tier-11", "Tirn's End", 11));
            maps.Add(new Map("Cold River Map", "cold-river-map-tier-11", "Lex Proxima", 11));
            maps.Add(new Map("Conservatory Map", "conservatory-map-tier-11", "Lex Ejoris", 11));
            maps.Add(new Map("Courthouse Map", "courthouse-map-tier-11", "Lira Arthain", 11));
            maps.Add(new Map("Courtyard Map", "courtyard-map-tier-11", "Lex Proxima", 11));
            maps.Add(new Map("Crater Map", "crater-map-tier-11", "Lex Proxima", 11));
            maps.Add(new Map("Crimson Temple Map", "crimson-temple-map-tier-11", "New Vastir", 11));
            maps.Add(new Map("Dark Forest Map", "dark-forest-map-tier-11", "Valdo's Rest", 11));
            maps.Add(new Map("Defiled Cathedral Map", "defiled-cathedral-map-tier-11", "Lex Proxima", 11));
            maps.Add(new Map("Desert Map", "desert-map-tier-11", "New Vastir", 11));
            maps.Add(new Map("Desert Spring Map", "desert-spring-map-tier-11", "Tirn's End", 11));
            maps.Add(new Map("Dunes Map", "dunes-map-tier-11", "New Vastir", 11));
            maps.Add(new Map("Fields Map", "fields-map-tier-11", "Tirn's End", 11));
            maps.Add(new Map("Flooded Mine Map", "flooded-mine-map-tier-11", "Lira Arthain", 11));
            maps.Add(new Map("Forbidden Woods Map", "forbidden-woods-map-tier-11", "Lira Arthain", 11));
            maps.Add(new Map("Fungal Hollow Map", "fungal-hollow-map-tier-11", "Haewark Hamlet", 11));
            maps.Add(new Map("Gardens Map", "gardens-map-tier-11", "Valdo's Rest", 11));
            maps.Add(new Map("Glacier Map", "glacier-map-tier-11", "Lex Proxima", 11));
            maps.Add(new Map("Grave Trough Map", "grave-trough-map-tier-11", "Haewark Hamlet", 11));
            maps.Add(new Map("Grotto Map", "grotto-map-tier-11", "Glennach Cairns", 11));
            maps.Add(new Map("Haunted Mansion Map", "haunted-mansion-map-tier-11", "Glennach Cairns", 11));
            maps.Add(new Map("Iceberg Map", "iceberg-map-tier-11", "Glennach Cairns", 11));
            maps.Add(new Map("Infested Valley Map", "infested-valley-map-tier-11", "Glennach Cairns", 11));
            maps.Add(new Map("Ivory Temple Map", "ivory-temple-map-tier-11", "Valdo's Rest", 11));
            maps.Add(new Map("Lair Map", "lair-map-tier-11", "Valdo's Rest", 11));
            maps.Add(new Map("Lava Chamber Map", "lava-chamber-map-tier-11", "Glennach Cairns", 11));
            maps.Add(new Map("Mineral Pools Map", "mineral-pools-map-tier-11", "Valdo's Rest", 11));
            maps.Add(new Map("Moon Temple Map", "moon-temple-map-tier-11", "Tirn's End", 11));
            maps.Add(new Map("Museum Map", "museum-map-tier-11", "Glennach Cairns", 11));
            maps.Add(new Map("Orchard Map", "orchard-map-tier-11", "Valdo's Rest", 11));
            maps.Add(new Map("Overgrown Ruin Map", "overgrown-ruin-map-tier-11", "Lex Proxima", 11));
            maps.Add(new Map("Overgrown Shrine Map", "overgrown-shrine-map-tier-11", "Lex Proxima", 11));
            maps.Add(new Map("Peninsula Map", "peninsula-map-tier-11", "Lex Proxima", 11));
            maps.Add(new Map("Port Map", "port-map-tier-11", "Tirn's End", 11));
            maps.Add(new Map("Precinct Map", "precinct-map-tier-11", "Lira Arthain", 11));
            maps.Add(new Map("Promenade Map", "promenade-map-tier-11", "Valdo's Rest", 11));
            maps.Add(new Map("Racecourse Map", "racecourse-map-tier-11", "Lira Arthain", 11));
            maps.Add(new Map("Ramparts Map", "ramparts-map-tier-11", "Haewark Hamlet", 11));
            maps.Add(new Map("Silo Map", "silo-map-tier-11", "Tirn's End", 11));
            maps.Add(new Map("Spider Forest Map", "spider-forest-map-tier-11", "Lex Proxima", 11));
            maps.Add(new Map("Stagnation Map", "stagnation-map-tier-11", "Glennach Cairns", 11));
            maps.Add(new Map("Sunken City Map", "sunken-city-map-tier-11", "Glennach Cairns", 11));
            maps.Add(new Map("Tower Map", "tower-map-tier-11", "Glennach Cairns", 11));
            maps.Add(new Map("Toxic Sewer Map", "toxic-sewer-map-tier-11", "Lira Arthain", 11));
            maps.Add(new Map("Tropical Island Map", "tropical-island-map-tier-11", "Glennach Cairns", 11));
            maps.Add(new Map("Underground River Map", "underground-river-map-tier-11", "Tirn's End", 11));
            maps.Add(new Map("Underground Sea Map", "underground-sea-map-tier-11", "Lex Proxima", 11));
            maps.Add(new Map("Vaal Pyramid Map", "vaal-pyramid-map-tier-11", "Lex Proxima", 11));
            maps.Add(new Map("Volcano Map", "volcano-map-tier-11", "Lex Proxima", 11));
            maps.Add(new Map("Wharf Map", "wharf-map-tier-11", "Tirn's End", 11));

            // tier 12
            maps.Add(new Map("Alleyways Map", "alleyways-map-tier-12", "Lex Ejoris", 12));
            maps.Add(new Map("Arsenal Map", "arsenal-map-tier-12", "Lex Ejoris", 12));
            maps.Add(new Map("Atoll Map", "atoll-map-tier-12", "Haewark Hamlet", 12));
            maps.Add(new Map("Basilica Map", "basilica-map-tier-12", "Glennach Cairns", 12));
            maps.Add(new Map("Bone Crypt Map", "bone-crypt-map-tier-12", "Lex Ejoris", 12));
            maps.Add(new Map("Canyon Map", "canyon-map-tier-12", "Valdo's Rest", 12));
            maps.Add(new Map("Cells Map", "cells-map-tier-12", "Lex Ejoris", 12));
            maps.Add(new Map("Cemetery Map", "cemetery-map-tier-12", "Haewark Hamlet", 12));
            maps.Add(new Map("Coral Ruins Map", "coral-ruins-map-tier-12", "Tirn's End", 12));
            maps.Add(new Map("Crimson Township Map", "crimson-township-map-tier-12", "Lex Proxima", 12));
            maps.Add(new Map("Crystal Ore Map", "crystal-ore-map-tier-12", "Lex Ejoris", 12));
            maps.Add(new Map("Desert Map", "desert-map-tier-12", "New Vastir", 12));
            maps.Add(new Map("Dig Map", "dig-map-tier-12", "Lex Ejoris", 12));
            maps.Add(new Map("Dungeon Map", "dungeon-map-tier-12", "Lex Ejoris", 12));
            maps.Add(new Map("Forking River Map", "forking-river-map-tier-12", "Lex Ejoris", 12));
            maps.Add(new Map("Frozen Cabins Map", "frozen-cabins-map-tier-12", "Lex Proxima", 12));
            maps.Add(new Map("Geode Map", "geode-map-tier-12", "Lex Proxima", 12));
            maps.Add(new Map("Ghetto Map", "ghetto-map-tier-12", "Valdo's Rest", 12));
            maps.Add(new Map("Graveyard Map", "graveyard-map-tier-12", "Glennach Cairns", 12));
            maps.Add(new Map("Laboratory Map", "laboratory-map-tier-12", "New Vastir", 12));
            maps.Add(new Map("Leyline Map", "leyline-map-tier-12", "Haewark Hamlet", 12));
            maps.Add(new Map("Lighthouse Map", "lighthouse-map-tier-12", "Lira Arthain", 12));
            maps.Add(new Map("Lookout Map", "lookout-map-tier-12", "Lira Arthain", 12));
            maps.Add(new Map("Malformation Map", "malformation-map-tier-12", "Glennach Cairns", 12));
            maps.Add(new Map("Marshes Map", "marshes-map-tier-12", "Lira Arthain", 12));
            maps.Add(new Map("Mausoleum Map", "mausoleum-map-tier-12", "Lira Arthain", 12));
            maps.Add(new Map("Mud Geyser Map", "mud-geyser-map-tier-12", "Tirn's End", 12));
            maps.Add(new Map("Palace Map", "palace-map-tier-12", "Haewark Hamlet", 12));
            maps.Add(new Map("Phantasmagoria Map", "phantasmagoria-map-tier-12", "Lex Proxima", 12));
            maps.Add(new Map("Pier Map", "pier-map-tier-12", "Lira Arthain", 12));
            maps.Add(new Map("Plaza Map", "plaza-map-tier-12", "Haewark Hamlet", 12));
            maps.Add(new Map("Primordial Blocks Map", "primordial-blocks-map-tier-12", "Lira Arthain", 12));
            maps.Add(new Map("Primordial Pool Map", "primordial-pool-map-tier-12", "Lex Ejoris", 12));
            maps.Add(new Map("Ramparts Map", "ramparts-map-tier-12", "Haewark Hamlet", 12));
            maps.Add(new Map("Reef Map", "reef-map-tier-12", "Lex Proxima", 12));
            maps.Add(new Map("Relic Chambers Map", "relic-chambers-map-tier-12", "New Vastir", 12));
            maps.Add(new Map("Shipyard Map", "shipyard-map-tier-12", "Lira Arthain", 12));
            maps.Add(new Map("Shore Map", "shore-map-tier-12", "Tirn's End", 12));
            maps.Add(new Map("Shrine Map", "shrine-map-tier-12", "New Vastir", 12));
            maps.Add(new Map("Siege Map", "siege-map-tier-12", "New Vastir", 12));
            maps.Add(new Map("Spider Lair Map", "spider-lair-map-tier-12", "Glennach Cairns", 12));
            maps.Add(new Map("Strand Map", "strand-map-tier-12", "New Vastir", 12));
            maps.Add(new Map("Sulphur Vents Map", "sulphur-vents-map-tier-12", "Lex Proxima", 12));
            maps.Add(new Map("Temple Map", "temple-map-tier-12", "Tirn's End", 12));
            maps.Add(new Map("Waste Pool Map", "waste-pool-map-tier-12", "Lex Ejoris", 12));
            maps.Add(new Map("Wasteland Map", "wasteland-map-tier-12", "Glennach Cairns", 12));
            maps.Add(new Map("Waterways Map", "waterways-map-tier-12", "Haewark Hamlet", 12));

            // tier 13
            maps.Add(new Map("Academy Map", "academy-map-tier-13", "Lex Proxima", 13));
            maps.Add(new Map("Alleyways Map", "alleyways-map-tier-13", "Lex Ejoris", 13));
            maps.Add(new Map("Armoury Map", "armoury-map-tier-13", "Lex Ejoris", 13));
            maps.Add(new Map("Channel Map", "channel-map-tier-13", "Haewark Hamlet", 13));
            maps.Add(new Map("Conservatory Map", "conservatory-map-tier-13", "Lex Ejoris", 13));
            maps.Add(new Map("Courthouse Map", "courthouse-map-tier-13", "Lira Arthain", 13));
            maps.Add(new Map("Crimson Temple Map", "crimson-temple-map-tier-13", "New Vastir", 13));
            maps.Add(new Map("Cursed Crypt Map", "cursed-crypt-map-tier-13", "Lex Proxima", 13));
            maps.Add(new Map("Dunes Map", "dunes-map-tier-13", "New Vastir", 13));
            maps.Add(new Map("Factory Map", "factory-map-tier-13", "Tirn's End", 13));
            maps.Add(new Map("Flooded Mine Map", "flooded-mine-map-tier-13", "Lira Arthain", 13));
            maps.Add(new Map("Fungal Hollow Map", "fungal-hollow-map-tier-13", "Haewark Hamlet", 13));
            maps.Add(new Map("Grave Trough Map", "grave-trough-map-tier-13", "Haewark Hamlet", 13));
            maps.Add(new Map("Iceberg Map", "iceberg-map-tier-13", "Glennach Cairns", 13));
            maps.Add(new Map("Mausoleum Map", "mausoleum-map-tier-13", "Lira Arthain", 13));
            maps.Add(new Map("Maze Map", "maze-map-tier-13", "Valdo's Rest", 13));
            maps.Add(new Map("Museum Map", "museum-map-tier-13", "Glennach Cairns", 13));
            maps.Add(new Map("Palace Map", "palace-map-tier-13", "Haewark Hamlet", 13));
            maps.Add(new Map("Park Map", "park-map-tier-13", "Lex Proxima", 13));
            maps.Add(new Map("Plateau Map", "plateau-map-tier-13", "Glennach Cairns", 13));
            maps.Add(new Map("Precinct Map", "precinct-map-tier-13", "Lira Arthain", 13));
            maps.Add(new Map("Racecourse Map", "racecourse-map-tier-13", "Lira Arthain", 13));
            maps.Add(new Map("Relic Chambers Map", "relic-chambers-map-tier-13", "New Vastir", 13));
            maps.Add(new Map("Scriptorium Map", "scriptorium-map-tier-13", "Glennach Cairns", 13));

            // tier 14
            maps.Add(new Map("Acid Caverns Map", "acid-caverns-map-tier-14", "Glennach Cairns", 14));
            maps.Add(new Map("Ancient City Map", "ancient-city-map-tier-14", "Glennach Cairns", 14));
            maps.Add(new Map("Arcade Map", "arcade-map-tier-14", "Glennach Cairns", 14));
            maps.Add(new Map("Arena Map", "arena-map-tier-14", "Valdo's Rest", 14));
            maps.Add(new Map("Arid Lake Map", "arid-lake-map-tier-14", "Valdo's Rest", 14));
            maps.Add(new Map("Ashen Wood Map", "ashen-wood-map-tier-14", "Valdo's Rest", 14));
            maps.Add(new Map("Atoll Map", "atoll-map-tier-14", "Haewark Hamlet", 14));
            maps.Add(new Map("Barrows Map", "barrows-map-tier-14", "Lira Arthain", 14));
            maps.Add(new Map("Basilica Map", "basilica-map-tier-14", "Glennach Cairns", 14));
            maps.Add(new Map("Beach Map", "beach-map-tier-14", "Lex Ejoris", 14));
            maps.Add(new Map("Belfry Map", "belfry-map-tier-14", "New Vastir", 14));
            maps.Add(new Map("Bramble Valley Map", "bramble-valley-map-tier-14", "Valdo's Rest", 14));
            maps.Add(new Map("Cage Map", "cage-map-tier-14", "Glennach Cairns", 14));
            maps.Add(new Map("Caldera Map", "caldera-map-tier-14", "Haewark Hamlet", 14));
            maps.Add(new Map("Carcass Map", "carcass-map-tier-14", "Lex Proxima", 14));
            maps.Add(new Map("Cemetery Map", "cemetery-map-tier-14", "Haewark Hamlet", 14));
            maps.Add(new Map("Chateau Map", "chateau-map-tier-14", "Tirn's End", 14));
            maps.Add(new Map("City Square Map", "city-square-map-tier-14", "Valdo's Rest", 14));
            maps.Add(new Map("Cold River Map", "cold-river-map-tier-14", "Lex Proxima", 14));
            maps.Add(new Map("Colosseum Map", "colosseum-map-tier-14", "New Vastir", 14));
            maps.Add(new Map("Coral Ruins Map", "coral-ruins-map-tier-14", "Tirn's End", 14));
            maps.Add(new Map("Core Map", "core-map-tier-14", "Haewark Hamlet", 14));
            maps.Add(new Map("Courtyard Map", "courtyard-map-tier-14", "Lex Proxima", 14));
            maps.Add(new Map("Coves Map", "coves-map-tier-14", "Tirn's End", 14));
            maps.Add(new Map("Crater Map", "crater-map-tier-14", "Lex Proxima", 14));
            maps.Add(new Map("Crystal Ore Map", "crystal-ore-map-tier-14", "Lex Ejoris", 14));
            maps.Add(new Map("Dark Forest Map", "dark-forest-map-tier-14", "Valdo's Rest", 14));
            maps.Add(new Map("Defiled Cathedral Map", "defiled-cathedral-map-tier-14", "Lex Proxima", 14));
            maps.Add(new Map("Desert Spring Map", "desert-spring-map-tier-14", "Tirn's End", 14));
            maps.Add(new Map("Dig Map", "dig-map-tier-14", "Lex Ejoris", 14));
            maps.Add(new Map("Dry Sea Map", "dry-sea-map-tier-14", "Valdo's Rest", 14));
            maps.Add(new Map("Dungeon Map", "dungeon-map-tier-14", "Lex Ejoris", 14));
            maps.Add(new Map("Excavation Map", "excavation-map-tier-14", "Haewark Hamlet", 14));
            maps.Add(new Map("Fields Map", "fields-map-tier-14", "Tirn's End", 14));
            maps.Add(new Map("Forbidden Woods Map", "forbidden-woods-map-tier-14", "Lira Arthain", 14));
            maps.Add(new Map("Forking River Map", "forking-river-map-tier-14", "Lex Ejoris", 14));
            maps.Add(new Map("Gardens Map", "gardens-map-tier-14", "Valdo's Rest", 14));
            maps.Add(new Map("Geode Map", "geode-map-tier-14", "Lex Proxima", 14));
            maps.Add(new Map("Glacier Map", "glacier-map-tier-14", "Lex Proxima", 14));
            maps.Add(new Map("Grotto Map", "grotto-map-tier-14", "Glennach Cairns", 14));
            maps.Add(new Map("Haunted Mansion Map", "haunted-mansion-map-tier-14", "Glennach Cairns", 14));
            maps.Add(new Map("Infested Valley Map", "infested-valley-map-tier-14", "Glennach Cairns", 14));
            maps.Add(new Map("Ivory Temple Map", "ivory-temple-map-tier-14", "Valdo's Rest", 14));
            maps.Add(new Map("Jungle Valley Map", "jungle-valley-map-tier-14", "Lex Proxima", 14));
            maps.Add(new Map("Laboratory Map", "laboratory-map-tier-14", "New Vastir", 14));
            maps.Add(new Map("Lair Map", "lair-map-tier-14", "Valdo's Rest", 14));
            maps.Add(new Map("Lava Chamber Map", "lava-chamber-map-tier-14", "Glennach Cairns", 14));
            maps.Add(new Map("Leyline Map", "leyline-map-tier-14", "Haewark Hamlet", 14));
            maps.Add(new Map("Lighthouse Map", "lighthouse-map-tier-14", "Lira Arthain", 14));
            maps.Add(new Map("Lookout Map", "lookout-map-tier-14", "Lira Arthain", 14));
            maps.Add(new Map("Malformation Map", "malformation-map-tier-14", "Glennach Cairns", 14));
            maps.Add(new Map("Mesa Map", "mesa-map-tier-14", "Tirn's End", 14));
            maps.Add(new Map("Mineral Pools Map", "mineral-pools-map-tier-14", "Valdo's Rest", 14));
            maps.Add(new Map("Moon Temple Map", "moon-temple-map-tier-14", "Tirn's End", 14));
            maps.Add(new Map("Orchard Map", "orchard-map-tier-14", "Valdo's Rest", 14));
            maps.Add(new Map("Overgrown Ruin Map", "overgrown-ruin-map-tier-14", "Lex Proxima", 14));
            maps.Add(new Map("Overgrown Shrine Map", "overgrown-shrine-map-tier-14", "Lex Proxima", 14));
            maps.Add(new Map("Pen Map", "pen-map-tier-14", "Valdo's Rest", 14));
            maps.Add(new Map("Peninsula Map", "peninsula-map-tier-14", "Lex Proxima", 14));
            maps.Add(new Map("Phantasmagoria Map", "phantasmagoria-map-tier-14", "Lex Proxima", 14));
            maps.Add(new Map("Plaza Map", "plaza-map-tier-14", "Haewark Hamlet", 14));
            maps.Add(new Map("Port Map", "port-map-tier-14", "Tirn's End", 14));
            maps.Add(new Map("Primordial Blocks Map", "primordial-blocks-map-tier-14", "Lira Arthain", 14));
            maps.Add(new Map("Primordial Pool Map", "primordial-pool-map-tier-14", "Lex Ejoris", 14));
            maps.Add(new Map("Promenade Map", "promenade-map-tier-14", "Valdo's Rest", 14));
            maps.Add(new Map("Ramparts Map", "ramparts-map-tier-14", "Haewark Hamlet", 14));
            maps.Add(new Map("Residence Map", "residence-map-tier-14", "Glennach Cairns", 14));
            maps.Add(new Map("Sepulchre Map", "sepulchre-map-tier-14", "Lex Ejoris", 14));
            maps.Add(new Map("Shipyard Map", "shipyard-map-tier-14", "Lira Arthain", 14));
            maps.Add(new Map("Shore Map", "shore-map-tier-14", "Tirn's End", 14));
            maps.Add(new Map("Shrine Map", "shrine-map-tier-14", "New Vastir", 14));
            maps.Add(new Map("Siege Map", "siege-map-tier-14", "New Vastir", 14));
            maps.Add(new Map("Silo Map", "silo-map-tier-14", "Tirn's End", 14));
            maps.Add(new Map("Spider Forest Map", "spider-forest-map-tier-14", "Lex Proxima", 14));
            maps.Add(new Map("Spider Lair Map", "spider-lair-map-tier-14", "Glennach Cairns", 14));
            maps.Add(new Map("Stagnation Map", "stagnation-map-tier-14", "Glennach Cairns", 14));
            maps.Add(new Map("Strand Map", "strand-map-tier-14", "New Vastir", 14));
            maps.Add(new Map("Sulphur Vents Map", "sulphur-vents-map-tier-14", "Lex Proxima", 14));
            maps.Add(new Map("Sunken City Map", "sunken-city-map-tier-14", "Glennach Cairns", 14));
            maps.Add(new Map("Temple Map", "temple-map-tier-14", "Tirn's End", 14));
            maps.Add(new Map("Thicket Map", "thicket-map-tier-14", "Lex Proxima", 14));
            maps.Add(new Map("Tower Map", "tower-map-tier-14", "Glennach Cairns", 14));
            maps.Add(new Map("Toxic Sewer Map", "toxic-sewer-map-tier-14", "Lira Arthain", 14));
            maps.Add(new Map("Tropical Island Map", "tropical-island-map-tier-14", "Glennach Cairns", 14));
            maps.Add(new Map("Underground River Map", "underground-river-map-tier-14", "Tirn's End", 14));
            maps.Add(new Map("Underground Sea Map", "underground-sea-map-tier-14", "Lex Proxima", 14));
            maps.Add(new Map("Vaal Pyramid Map", "vaal-pyramid-map-tier-14", "Lex Proxima", 14));
            maps.Add(new Map("Volcano Map", "volcano-map-tier-14", "Lex Proxima", 14));
            maps.Add(new Map("Waste Pool Map", "waste-pool-map-tier-14", "Lex Ejoris", 14));
            maps.Add(new Map("Wasteland Map", "wasteland-map-tier-14", "Glennach Cairns", 14));
            maps.Add(new Map("Waterways Map", "waterways-map-tier-14", "Haewark Hamlet", 14));
            maps.Add(new Map("Wharf Map", "wharf-map-tier-14", "Tirn's End", 14));

            // tier 15
            maps.Add(new Map("Alleyways Map", "alleyways-map-tier-15", "Lex Ejoris", 15));
            maps.Add(new Map("Arachnid Tomb Map", "arachnid-tomb-map-tier-15", "New Vastir", 15));
            maps.Add(new Map("Armoury Map", "armoury-map-tier-15", "Lex Ejoris", 15));
            maps.Add(new Map("Arsenal Map", "arsenal-map-tier-15", "Lex Ejoris", 15));
            maps.Add(new Map("Bazaar Map", "bazaar-map-tier-15", "Tirn's End", 15));
            maps.Add(new Map("Bog Map", "bog-map-tier-15", "Lex Ejoris", 15));
            maps.Add(new Map("Bone Crypt Map", "bone-crypt-map-tier-15", "Lex Ejoris", 15));
            maps.Add(new Map("Canyon Map", "canyon-map-tier-15", "Valdo's Rest", 15));
            maps.Add(new Map("Castle Ruins Map", "castle-ruins-map-tier-15", "Lira Arthain", 15));
            maps.Add(new Map("Cells Map", "cells-map-tier-15", "Lex Ejoris", 15));
            maps.Add(new Map("Channel Map", "channel-map-tier-15", "Haewark Hamlet", 15));
            maps.Add(new Map("Colonnade Map", "colonnade-map-tier-15", "Tirn's End", 15));
            maps.Add(new Map("Conservatory Map", "conservatory-map-tier-15", "Lex Ejoris", 15));
            maps.Add(new Map("Courthouse Map", "courthouse-map-tier-15", "Lira Arthain", 15));
            maps.Add(new Map("Crimson Temple Map", "crimson-temple-map-tier-15", "New Vastir", 15));
            maps.Add(new Map("Crimson Township Map", "crimson-township-map-tier-15", "Lex Proxima", 15));
            maps.Add(new Map("Desert Map", "desert-map-tier-15", "New Vastir", 15));
            maps.Add(new Map("Dunes Map", "dunes-map-tier-15", "New Vastir", 15));
            maps.Add(new Map("Estuary Map", "estuary-map-tier-15", "Lira Arthain", 15));
            maps.Add(new Map("Flooded Mine Map", "flooded-mine-map-tier-15", "Lira Arthain", 15));
            maps.Add(new Map("Foundry Map", "foundry-map-tier-15", "New Vastir", 15));
            maps.Add(new Map("Frozen Cabins Map", "frozen-cabins-map-tier-15", "Lex Proxima", 15));
            maps.Add(new Map("Fungal Hollow Map", "fungal-hollow-map-tier-15", "Haewark Hamlet", 15));
            maps.Add(new Map("Ghetto Map", "ghetto-map-tier-15", "Valdo's Rest", 15));
            maps.Add(new Map("Grave Trough Map", "grave-trough-map-tier-15", "Haewark Hamlet", 15));
            maps.Add(new Map("Graveyard Map", "graveyard-map-tier-15", "Glennach Cairns", 15));
            maps.Add(new Map("Lava Lake Map", "lava-lake-map-tier-15", "Lex Proxima", 15));
            maps.Add(new Map("Marshes Map", "marshes-map-tier-15", "Lira Arthain", 15));
            maps.Add(new Map("Museum Map", "museum-map-tier-15", "Glennach Cairns", 15));
            maps.Add(new Map("Palace Map", "palace-map-tier-15", "Haewark Hamlet", 15));
            maps.Add(new Map("Pier Map", "pier-map-tier-15", "Lira Arthain", 15));
            maps.Add(new Map("Plateau Map", "plateau-map-tier-15", "Glennach Cairns", 15));
            maps.Add(new Map("Precinct Map", "precinct-map-tier-15", "Lira Arthain", 15));
            maps.Add(new Map("Racecourse Map", "racecourse-map-tier-15", "Lira Arthain", 15));
            maps.Add(new Map("Reef Map", "reef-map-tier-15", "Lex Proxima", 15));
            maps.Add(new Map("Relic Chambers Map", "relic-chambers-map-tier-15", "New Vastir", 15));
            maps.Add(new Map("Scriptorium Map", "scriptorium-map-tier-15", "Glennach Cairns", 15));
            maps.Add(new Map("Summit Map", "summit-map-tier-15", "Haewark Hamlet", 15));
            maps.Add(new Map("Terrace Map", "terrace-map-tier-15", "Haewark Hamlet", 15));
            maps.Add(new Map("Vault Map", "vault-map-tier-15", "New Vastir", 15));
            maps.Add(new Map("Villa Map", "villa-map-tier-15", "Glennach Cairns", 15));
            maps.Add(new Map("Waterways Map", "waterways-map-tier-15", "Haewark Hamlet", 15));

            // tier 16
            maps.Add(new Map("Academy Map", "academy-map-tier-16", "Lex Proxima", 16));
            maps.Add(new Map("Arachnid Nest Map", "arachnid-nest-map-tier-16", "Haewark Hamlet", 16));
            maps.Add(new Map("Atoll Map", "atoll-map-tier-16", "Haewark Hamlet", 16));
            maps.Add(new Map("Barrows Map", "barrows-map-tier-16", "Lira Arthain", 16));
            maps.Add(new Map("Beach Map", "beach-map-tier-16", "Lex Ejoris", 16));
            maps.Add(new Map("Burial Chambers Map", "burial-chambers-map-tier-16", "Lex Ejoris", 16));
            maps.Add(new Map("Cage Map", "cage-map-tier-16", "Glennach Cairns", 16));
            maps.Add(new Map("Caldera Map", "caldera-map-tier-16", "Haewark Hamlet", 16));
            maps.Add(new Map("Colosseum Map", "colosseum-map-tier-16", "New Vastir", 16));
            maps.Add(new Map("Cursed Crypt Map", "cursed-crypt-map-tier-16", "Lex Proxima", 16));
            maps.Add(new Map("Dungeon Map", "dungeon-map-tier-16", "Lex Ejoris", 16));
            maps.Add(new Map("Excavation Map", "excavation-map-tier-16", "Haewark Hamlet", 16));
            maps.Add(new Map("Factory Map", "factory-map-tier-16", "Tirn's End", 16));
            maps.Add(new Map("Iceberg Map", "iceberg-map-tier-16", "Glennach Cairns", 16));
            maps.Add(new Map("Lighthouse Map", "lighthouse-map-tier-16", "Lira Arthain", 16));
            maps.Add(new Map("Mausoleum Map", "mausoleum-map-tier-16", "Lira Arthain", 16));
            maps.Add(new Map("Maze Map", "maze-map-tier-16", "Valdo's Rest", 16));
            maps.Add(new Map("Mud Geyser Map", "mud-geyser-map-tier-16", "Tirn's End", 16));
            maps.Add(new Map("Necropolis Map", "necropolis-map-tier-16", "Valdo's Rest", 16));
            maps.Add(new Map("Park Map", "park-map-tier-16", "Lex Proxima", 16));
            maps.Add(new Map("Pit Map", "pit-map-tier-16", "New Vastir", 16));
            maps.Add(new Map("Shipyard Map", "shipyard-map-tier-16", "Lira Arthain", 16));
            maps.Add(new Map("Shrine Map", "shrine-map-tier-16", "New Vastir", 16));
            maps.Add(new Map("Siege Map", "siege-map-tier-16", "New Vastir", 16));
            maps.Add(new Map("Vaal Temple Map", "vaal-temple-map-tier-16", "Lex Proxima", 16));
            maps.Add(new Map("Waste Pool Map", "waste-pool-map-tier-16", "Lex Ejoris", 16));
            maps.Add(new Map("Wasteland Map", "wasteland-map-tier-16", "Glennach Cairns", 16));



        }

        public String listMaps()
        {
            String str = "";

            foreach(Map m in maps)
            {
                str += m.ToString() + "\n";

            }

            return str;
        }

        public String listMapsByTier(int tier)
        {
            String str = "";

            foreach (Map m in maps)
            {
                if(m.tier == tier)
                    str += m.ToString() + "\n";

            }

            return str;
        }

        public List<Map> getMapsByTier(List<Map> selected_maps, int tier)
        {
            List<Map> mapsbytier = new List<Map>();
            foreach (Map m in selected_maps)
            {
                if (m.tier == tier)
                    mapsbytier.Add(m);

            }

            return mapsbytier;
        }

        public void defineWatchStones()
        {

            Dictionary<String, int> ocurrencias = new Dictionary<string, int>();

            for(int i = 16; i > 0; i--)
            {
                List<Map> aux_maps = new List<Map>();
                foreach(Map m in maps)
                {
                    if(m.tier == i)
                    {
                        aux_maps.Add(m);
                    }
                }

                foreach(Map m in aux_maps)
                {
                    if (ocurrencias.ContainsKey(m.name))
                    {
                        int value = ocurrencias[m.name];
                        m.watchstones = 4 - value;
                        ocurrencias[m.name] = value + 1;
                        //Console.WriteLine("before value: "+value + "; after value: "+ocurrencias[m.name]);
                    }
                    else
                    {
                        ocurrencias.Add(m.name, 1);
                        m.watchstones = 4;
                    }
                }

            }
        }

        public Button getMapButton(Map m)
        {
            Button mbutton = new Button();

            mbutton.Content = m.name + "\n["+m.tier+"] " + m.zone+"  ("+m.watchstones+")";
            mbutton.FontSize = 16;
            mbutton.FontWeight = FontWeights.Bold;
            mbutton.Margin = new Thickness(8, 8, 8, 8);
            //mbutton.Width = 200;
            mbutton.HorizontalContentAlignment = HorizontalAlignment.Left;
            
            if(m.tier <= 5)
            {
                mbutton.Background = Brushes.White;
            }
            else if(m.tier <= 10)
            {
                mbutton.Background = Brushes.LightGoldenrodYellow;
            }
            else
            {
                mbutton.Background = Brushes.IndianRed;
            }
            mbutton.DataContext = m;
            mbutton.BorderThickness = new Thickness(3);
            if(selected_maps.Contains(m))
                mbutton.BorderBrush = Brushes.DarkBlue;
            else
                mbutton.BorderBrush = Brushes.LightBlue;
            mbutton.AddHandler(Button.ClickEvent, new RoutedEventHandler(selectMapButtonEvent));


            return mbutton;
        }

        public void updateSelectedMapsGrid()
        {
            Grid myGrid = selectedMapsGrid;
            myGrid.Children.Clear();
            bool secondColumn = false;
            int row = 0;
            

            foreach(Map m in selected_maps)
            {
                Button b = getMapButton(m);
                b.Height = 55;
                //b.BorderBrush = Brushes.DarkBlue;

                if (!secondColumn)
                {

                    Grid.SetRow(b, row);
                    Grid.SetColumn(b, 0);
                    secondColumn = true;
                }
                else
                {
                    Grid.SetRow(b, row);
                    Grid.SetColumn(b, 1);

                    selectedMapsGrid.RowDefinitions.Add(new RowDefinition());
                    row++;

                    secondColumn = false;

                }
                myGrid.Children.Add(b);


            }
        }

        void selectMapButtonEvent(object sender, EventArgs e)
        {
            Button b = sender as Button;

            Map m = (Map)b.DataContext;


            if (b.BorderBrush == Brushes.LightBlue)
            {
                selected_maps.Add(m);
                b.BorderBrush = Brushes.DarkBlue;
            }
            else
            {
                selected_maps.Remove(m);
                b.BorderBrush = Brushes.LightBlue;
            }

            updateSelectedMapsGrid();
            listMapsInGrid(filteredMaps);
        }

        void selectHideButtonEvent(object sender, EventArgs e)
        {
            Button b = sender as Button;
            Grid tierGrid = b.DataContext as Grid;
            if (b.Content.Equals("+"))
            {
                b.Content = "-";
                tierGrid.Visibility = Visibility.Visible;
                tiers_shown[(int)b.Tag] = true;
            }
            else
            {
                b.Content = "+";
                tierGrid.Visibility = Visibility.Collapsed;
                tiers_shown[(int)b.Tag] = false;
            }


        }

        public void listMapsInGrid(List<Map> filt_maps)
        {
            Grid myGrid = MapsGrid;
            myGrid.Children.Clear();
            int rowsOccupied = 0;
            for(int i = 0; i < 16; i++)
            {

                List<Map> mapsbytier = getMapsByTier(filt_maps, i + 1);
                if (mapsbytier.Count() == 0)
                    continue;
                RowDefinition rowDef1 = new RowDefinition();
                myGrid.RowDefinitions.Add(rowDef1);

                TextBlock txt1 = new TextBlock();
                txt1.Text = "Tier "+(i+1)+":";
                txt1.FontSize = 24;
                txt1.Margin = new Thickness(50, 30, 0, 10);
                txt1.FontWeight = FontWeights.Bold;

                Button hideButton = new Button();

                hideButton.FontSize = 20;
                hideButton.Width = 30;
                hideButton.Margin = new Thickness(8, 30, 0, 10);
                hideButton.BorderThickness = new Thickness(4);
                hideButton.BorderBrush = Brushes.Gold;
                hideButton.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetRow(hideButton, rowsOccupied * 2);
                Grid.SetRow(txt1, rowsOccupied * 2);




                RowDefinition rowDef2 = new RowDefinition();
                myGrid.RowDefinitions.Add(rowDef2);

                Grid tierGrid = new Grid();

                hideButton.DataContext = tierGrid;
                hideButton.Tag = i + 1;


                for(int k = 0; k < 5; k++)
                {
                    ColumnDefinition colDef1 = new ColumnDefinition();
                    tierGrid.ColumnDefinitions.Add(colDef1);
                }

                int line = 0;

                RowDefinition rowDef3 = new RowDefinition();
                tierGrid.RowDefinitions.Add(rowDef3);
                for (int j = 0; j < mapsbytier.Count; j++)
                {

                    Button mapButton = getMapButton(mapsbytier[j]);

                    Grid.SetRow(mapButton, line);
                    Grid.SetColumn(mapButton, (j - (line * 5)));
                    tierGrid.Children.Add(mapButton);

                    if (j == ((line * 5) + 4))
                    {
                        line++;
                        RowDefinition rowDef4 = new RowDefinition();
                        tierGrid.RowDefinitions.Add(rowDef4);
                    }

                }

                Grid.SetRow(tierGrid, (rowsOccupied * 2) + 1);

                hideButton.AddHandler(Button.ClickEvent, new RoutedEventHandler(selectHideButtonEvent));

                myGrid.Children.Add(txt1);
                myGrid.Children.Add(hideButton);
                myGrid.Children.Add(tierGrid);
                if (!tiers_shown[i + 1])
                {
                    tierGrid.Visibility = Visibility.Collapsed;
                    hideButton.Content = "+";
                }
                else
                {
                    tierGrid.Visibility = Visibility.Visible;
                    hideButton.Content = "-";
                }
                rowsOccupied++;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializeDictionary();
            InitializeMaps();
            defineWatchStones();
            filteredMaps = maps;
            listMapsInGrid(filteredMaps);
        }

        public void updateFilteredMaps()
        {
            filteredMaps = new List<Map>();
            if(selected_zones.Count == 8 && selected_watchstones == -1)
            {
                filteredMaps = maps;
                Console.WriteLine("every map selected: "+filteredMaps.Count()+" maps!");
            }
            else if(selected_watchstones == -1)
            {
                foreach(Map m in maps)
                {
                    if (selected_zones.Contains(m.zone))
                    {
                        //Console.WriteLine(m.name + "(t"+m.tier+") is from " + m.zone + ", which is selected");
                        filteredMaps.Add(m);
                    }
                }


                Console.WriteLine("Some zones selected: " + filteredMaps.Count() + " maps!");

            }
            else
            {
                foreach (Map m in maps)
                {
                    if (selected_watchstones == m.watchstones && selected_zones.Contains(m.zone))
                    {
                        Console.WriteLine(m.name + " (t" + m.tier + ") is from " + m.zone + " and has "+selected_watchstones);
                        filteredMaps.Add(m);
                    }
                }


                Console.WriteLine("Some zones selected: " + filteredMaps.Count() + " maps!");

            }



            listMapsInGrid(filteredMaps);

        }

        private void click_update_maps(object sender, RoutedEventArgs e)
        {

            selected_zones.Clear();
            if (check_zone_glennach_cairns.IsChecked ?? false)
                selected_zones.Add(Regions.glennach_cairns);
            if (check_zone_haewark_hamlet.IsChecked ?? false)
                selected_zones.Add(Regions.haewark_hamlet);
            if (check_zone_lex_proxima.IsChecked ?? false)
                selected_zones.Add(Regions.lex_proxima);
            if (check_zone_lex_ejoris.IsChecked ?? false)
                selected_zones.Add(Regions.lex_ejoris);
            if (check_zone_lira_arthain.IsChecked ?? false)
                selected_zones.Add(Regions.lira_arthain);
            if (check_zone_new_vastir.IsChecked ?? false)
                selected_zones.Add(Regions.new_vastir);
            if (check_zone_tirns_end.IsChecked ?? false)
                selected_zones.Add(Regions.tirns_end);
            if (check_zone_valdos_rest.IsChecked ?? false)
                selected_zones.Add(Regions.valdos_rest);

            if (radioStone_0.IsChecked ?? false)
            {
                selected_watchstones = 0;
            }
            else if(radioStone_1.IsChecked ?? false)
            {
                selected_watchstones = 1;
            }
            else if (radioStone_2.IsChecked ?? false)
            {
                selected_watchstones = 2;
            }
            else if (radioStone_3.IsChecked ?? false)
            {
                selected_watchstones = 3;
            }
            else if(radioStone_4.IsChecked ?? false)
            {
                selected_watchstones = 4;
            }
            else if(radioStone_all.IsChecked ?? false)
            {
                selected_watchstones = -1;
            }

            updateFilteredMaps();

        }

        public void startSearchTask()
        {

            request = new RequestResult(logsList, requestGrid);
            RequestClient rq = RequestClient.GetInstance();
            Application.Current.Dispatcher.Invoke((Action)delegate {
                logInfo("Starting search for " + selected_maps.Count() + " maps!");
            });

            foreach (Map m in selected_maps)
            {
                var watch = Stopwatch.StartNew();

                Application.Current.Dispatcher.Invoke((Action)delegate {
                    logInfo("Getting offers for map \"" + m.name +"!");
                });

                PostResult pr = rq.postToGetFirst100Maps(m);
                request.addPostResult(pr);

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;

                Application.Current.Dispatcher.Invoke((Action)delegate {
                    logInfo("Getting offers for map \"" + m.name + "\" took " + (int)elapsedMs);
                });
                if (elapsedMs < 10000)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate {
                        logInfo("Sleeping for another " + (10000 - (int)elapsedMs) + " seconds to prevent timeout!");
                    });
                    Thread.Sleep(10000 - (int)elapsedMs);
                }

            }
            
        }

        private void send_request_click(object sender, RoutedEventArgs e)
        {
            if (searchTaskRunning)
            {
                logError("Cant start another research while waiting for one...");
                return;
            }

            requestGrid.Children.Clear();
            ((Button)sender).Background = Brushes.DarkRed;
            Task task = new Task(new Action(startSearchTask));
            Task continuationTask = task.ContinueWith(t => searchTaskRunning = false)
                .ContinueWith(t => Application.Current.Dispatcher.Invoke((Action)delegate {
                    logInfo("Ready to start another search!\n");
                }))
                .ContinueWith(t => this.Dispatcher.Invoke(() =>
                {
                    ((Button)sender).Background = Brushes.White;
                }));
            searchTaskRunning = true;
            task.Start();




        }
    }
}
