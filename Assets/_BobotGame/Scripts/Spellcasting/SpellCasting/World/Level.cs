using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
namespace SpellCasting.World
{
    public class Level: MonoBehaviour
    {
        [SerializeField]
        private List<Room> existingRooms;
        //public List<Room> Rooms => rooms;

        [SerializeField]
        private List<float> TEMPStartingCreditses = new List<float> { 2, 4, 6 };

        [SerializeField]
        private float availableCredits;

        private Dictionary<UniqueRoom, int> _uniqueRoomsSpawned;
        private List<Room> _existingRoomQueue;

        [SerializeField]
        private bool DebugShowOverlaps;

        void Start()
        {
            InitializeValues();
            GenerateLevel();
        }

        public void InitializeValues()
        {
            bool hasAir = MainGame.Instance.SavedData.UnlockedElements.Contains(ElementCatalog.Instance.ElementTypesMap[ElementTypeIndex.AIR]);
            int progression = Mathf.Max(LevelProgressionManager.DifficultyProgression, hasAir ? 0 : 1);
            availableCredits = TEMPStartingCreditses[Mathf.Clamp(progression, 0, TEMPStartingCreditses.Count - 1)];

            _uniqueRoomsSpawned = new Dictionary<UniqueRoom, int>();
        }

        public void GenerateLevel()
        {
            _existingRoomQueue = new List<Room>(existingRooms);

            float lowestCost = float.MaxValue;
            for (int i = 0; i < RoomCatalog.Instance.AllAvailableRooms.Count; i++)
            {
                float cost = RoomCatalog.Instance.AllAvailableRooms[i].RoomCost;
                if (cost > 0)
                {
                    lowestCost = RoomCatalog.Instance.AllAvailableRooms[i].RoomCost;
                }
            }

            int failsafe = 0;
            while (_existingRoomQueue.Count > 0 && (availableCredits > lowestCost || !AreAllRequiredRoomsSpawned()))
            {
                failsafe++;
                if (failsafe > 10000)
                {
                    bool breakpointHere = true;
                    breakpointHere = !breakpointHere;
                }

                if (failsafe > 20000)
                {
                    Debug.LogError($"FAILSAFE: credits {availableCredits}, roomqueue {_existingRoomQueue.Count}");
                    break;
                }

                _existingRoomQueue.Shuffle();

                List<Door> allOpenDoors = new List<Door>();
                for (int i = 0; i < _existingRoomQueue.Count; i++)
                {
                    for (int j = 0; j < _existingRoomQueue[i].Doors.Count; j++)
                    {
                        if (!_existingRoomQueue[i].Doors[j].Occupied)
                        {
                            allOpenDoors.Add(_existingRoomQueue[i].Doors[j]);
                        }
                    }
                }

                //for this existing room check its existing doors
                Room existingRoom = _existingRoomQueue[0];
                existingRoom.Doors.Shuffle();
                for (int eD = 0; eD < existingRoom.Doors.Count; eD++)
                {
                    Door existingDoor = existingRoom.Doors[eD];

                    int occupiedDoors = 0;
                    if (existingDoor.Occupied)
                    {
                        occupiedDoors++;
                        //all doors occupied, remove this room
                        if (eD == existingRoom.Doors.Count - 1 && occupiedDoors >= existingRoom.Doors.Count - 1)
                        {
                            _existingRoomQueue.Remove(existingRoom);
                            break;
                        }
                    }

                    List<Room> potentialRooms;
                    if (availableCredits < lowestCost && !AreAllRequiredRoomsSpawned())
                    {
                        //only spawn requried rooms now
                        potentialRooms = new List<Room>();
                    }
                    else
                    {
                        //spawn rooms based on currency (aka continue as normal)
                        potentialRooms = new List<Room>(RoomCatalog.Instance.AllAvailableRooms);
                        for (int r = potentialRooms.Count - 1; r >= 0; r--)
                        {
                            if (potentialRooms[r].RoomCost > availableCredits)
                            {
                                potentialRooms.RemoveAt(r);
                            }
                        }
                    }
                    AddUniqueRooms(potentialRooms);

                    //for each existing door check potential rooms
                    int tries = 0;
                    while (potentialRooms.Count > 0)
                    {
                        tries++;
                        if (tries > 20000)
                        {
                            Debug.LogError($"FAILSAFE: credits {availableCredits}, roomqueue {_existingRoomQueue.Count}, potentialRooms{potentialRooms.Count}");
                            break;
                        }

                        Room potentialRoomPrefab = potentialRooms.WeightedRandom((room) => room.RoomWeight);

                        if (potentialRoomPrefab == null)
                        {
                            potentialRooms.Clear();
                            Debug.LogError("could not get any rooms to randomize");
                            return;
                        }

                        Room newRoom = TrySpawnRoom(existingDoor, potentialRoomPrefab);

                        if (newRoom != null)
                        {
                            existingRooms.Add(newRoom);
                            _existingRoomQueue.Add(newRoom);
                            IncrementUniqueRoomAndCheckRemove(potentialRoomPrefab, potentialRooms);
                            break;
                        }
                        else
                        {
                            //if the potential room was unsuccessful, remove it from the list and try a different one
                            potentialRooms.Remove(potentialRoomPrefab);
                            continue;
                        }
                    }

                    //if there are no more rooms left to check (all of them have failed)
                    if (potentialRooms.Count == 0)
                    {
                        allOpenDoors.Remove(existingDoor);
                    }
                }
                //no more rooms possible. uh just leave?
                if (allOpenDoors.Count == 0)
                {
                    Debug.LogError("it is physically impossible to place any more rooms. well shit");
                    break;
                }
            }

            //after generation, close all remaining doors
            for (int r = 0; r < existingRooms.Count; r++)
            {
                var ROom = existingRooms[r];
                for (int rD = 0; rD < ROom.Doors.Count; rD++)
                {
                    if (!ROom.Doors[rD].Occupied)
                    {
                        ROom.Doors[rD].SetOpen(false);
                    }
                }
            }
#if UNITY_EDITOR
            //LogReadout();
#endif
        }

        private Room TrySpawnRoom(Door existingDoor, Room potentialRoomPrefab)
        {
            Room newRoom = null;
            //for each potential room look at their doors
            List<Door> potentialRoomDoors = new List<Door>(potentialRoomPrefab.Doors);
            potentialRoomDoors.Shuffle();
            for (int pD = 0; pD < potentialRoomDoors.Count; pD++)
            {
                Door potentialDoor = potentialRoomDoors[pD];
                if ((int)potentialDoor.FacingDirection != -(int)existingDoor.FacingDirection)
                    continue;

                bool overlapped = false;

                //check the overlapzones of the potential room with respect to the potential door aligned to the existing door
                for (int m = 0; m < potentialRoomPrefab.OverlapZones.Count; m++)
                {
                    OverlapZone overlapZone = potentialRoomPrefab.OverlapZones[m];
                    Vector3 center = overlapZone.localPosition - potentialDoor.transform.localPosition + existingDoor.transform.position;

                    Collider[] overlaps = Physics.OverlapBox(center, overlapZone.localScale * 0.5f, Quaternion.identity, LayerInfo.RoomOverlap.layerMask);
                    overlapped |= overlaps.Length > 0;
#if UNITY_EDITOR
                     if(DebugShowOverlaps) DebugOverlapCube(existingDoor, potentialRoomPrefab, potentialDoor, overlapZone, center, overlaps);
#endif
                }

                //if there are no overlaps, spawn the new room
                if (!overlapped)
                {
                    Vector3 position = potentialRoomPrefab.transform.position - potentialDoor.transform.position + existingDoor.transform.position;

                    newRoom = Instantiate(potentialRoomPrefab, position, Quaternion.identity, transform);

                    newRoom.Doors[potentialDoor.DoorIndex].Occupied = true;
                    newRoom.Doors[potentialDoor.DoorIndex].SetOpen(true);

                    existingDoor.Occupied = true;
                    existingDoor.SetOpen(true);
                    availableCredits -= potentialRoomPrefab.RoomCost;

                    break;
                }
            }

            return newRoom;
        }

        // I've passed 200 script lines noooooo
        private bool AreAllRequiredRoomsSpawned()
        {
            for (int i = 0; i < RoomCatalog.Instance.AllUniqueRooms.Count; i++)
            {
                UniqueRoom room = RoomCatalog.Instance.AllUniqueRooms[i];

                if (_uniqueRoomsSpawned.TryGetValueDefault(room) < room.minimumInstancesRequired)
                {
                    return false;
                }
            }
            return true;
        }

        private void AddUniqueRooms(List<Room> potentialRooms)
        {
            for (int i = 0; i < RoomCatalog.Instance.AllUniqueRooms.Count; i++)
            {
                UniqueRoom uniqueRoom = RoomCatalog.Instance.AllUniqueRooms[i];

                if(_uniqueRoomsSpawned.ContainsKey(uniqueRoom) && _uniqueRoomsSpawned[uniqueRoom] >= uniqueRoom.maximumInstancesAllowed)
                    continue;

                potentialRooms.Add(uniqueRoom.room);
            }
        }

        public void IncrementUniqueRoomAndCheckRemove(Room newRoomPrefab, List<Room> potentialRooms)
        {
            for (int i = 0; i < RoomCatalog.Instance.AllUniqueRooms.Count; i++)
            {
                if (RoomCatalog.Instance.AllUniqueRooms[i].room == newRoomPrefab)
                {
                    Util.IncrementvValue(_uniqueRoomsSpawned, RoomCatalog.Instance.AllUniqueRooms[i], 1);
                }
            }

            foreach (KeyValuePair<UniqueRoom, int> uniqueRoomCounts in _uniqueRoomsSpawned)
            {
                if (uniqueRoomCounts.Key.room == newRoomPrefab)
                {
                    if (_uniqueRoomsSpawned[uniqueRoomCounts.Key] >= uniqueRoomCounts.Key.maximumInstancesAllowed)
                    {
                        potentialRooms.Remove(newRoomPrefab);
                    }
                }
            }
        }

        private void DebugOverlapCube(Door existingDoor, Room potentialRoom, Door potentialDoor, OverlapZone overlapZone, Vector3 center, Collider[] overlaps)
        {
            Debug.LogWarning($"p room {potentialRoom.name}, checking p overlapzone {overlapZone} with respect to p door {potentialDoor.name}," +
                $"checked against e door {existingDoor.name} and found {overlaps.Length} overlaps");
            TestCube(overlaps.Length > 0, center, overlapZone.localScale, $"p{potentialRoom.name} p{overlapZone.name} p {potentialDoor.name} e {existingDoor.name}");
        }

        #region debug

        [ContextMenu("ReGenerate")]
        public void RegenerateLevel()
        {
            InitializeValues();
            for (int i = 0; i < existingRooms.Count; i++)
            {
                Destroy(existingRooms[i].gameObject);
            }
            existingRooms = new List<Room>
            {
                Instantiate(RoomCatalog.Instance.StartRoom, Vector3.zero, Quaternion.identity, transform)
            };

            Invoke("GenerateLevel", 0.2f);
        }

        public void TestCube(bool overlapped, Vector3 position, Vector3 scale, string name)
        {
            var cob = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cob.name = name;
            cob.transform.position = position;
            cob.transform.localScale = scale;

            if (overlapped)
            {
                cob.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.magenta);
            }
        }
        Dictionary<string, int> roomLog = new Dictionary<string, int>();

#if UNITY_EDITOR

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                RegenerateLevel();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                roomLog = new Dictionary<string, int>();
            }
        }
#endif
        private void LogReadout()
        {
            for (int i = 0; i < existingRooms.Count; i++)
            {
                if (!roomLog.ContainsKey(existingRooms[i].name))
                {
                    roomLog[existingRooms[i].name] = 0;
                }
                roomLog[existingRooms[i].name]++;
            }

            string readout = "room distribution:";
            foreach (var lo in roomLog)
            {
                readout += $"\n{lo.Key}: {lo.Value}";
            }
            Debug.Log(readout);
        }
        #endregion debug
    }
}