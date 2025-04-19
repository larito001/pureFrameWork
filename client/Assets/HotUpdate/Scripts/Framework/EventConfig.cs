using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YOTO
{    
    public enum EventType
    {
       //input :
       Move,

       Sprint,
       //UI
       Loading,
       //touch
       Touch,
       TouchRelease,
       TouchMove,
       Scroll,
       UseItem,
       Cooking,
       
       RefreshMousePos,
       
       //net:
       RoomListResponse,
       LoginResponse,
       CreatRoom,
       JoinRoomResponse,
       UpdateRoleData,
       RefreshRoomRoleUI,
       StartGame,

       //
       RefreshShopThings,
       RefreshMoney,
       RefreshDate,
       RefreshInsurance,
       RefreshNextNews,
       RefreshOrder,
       RefreshStock,
       RefreshStockRate,
       //效果
       
       SetMoveSpeed,
       
       
       //游戏循环
       StartPlaying,
       GameCircleEnd,
       
       //突发事件：
       CustomDie,
       FoodMixFailure,
    }


}
