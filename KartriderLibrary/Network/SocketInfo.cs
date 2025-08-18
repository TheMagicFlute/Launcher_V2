using System;
using System.Net.Sockets;

namespace KartRider.Common.Network
{
    public class SocketInfo
    {
        public readonly Socket Socket;

        public bool NoEncryption;

        public SocketInfo.StateEnum State;

        public byte[] DataBuffer;

        public int Index;

        SocketInfo()
        {
            this.DataBuffer = null;
        }

        public SocketInfo(Socket socket, short headerLength, bool noEncryption = false)
        {
            this.Socket = socket;
            this.State = SocketInfo.StateEnum.Header;
            this.NoEncryption = noEncryption;
            this.DataBuffer = new byte[headerLength];
            this.Index = 0;
        }

        public enum StateEnum
        {
            Header,
            Content
        }
    }
}