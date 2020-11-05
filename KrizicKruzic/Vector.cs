using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrizicKruzic
{
	class Vec2i
	{
		public int X { get; set; }
		public int Y { get; set; }

		//Konstruktori
		public Vec2i() : this(0, 0) { }
		public Vec2i(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		//Public metode
		public void Offset(int x, int y)
		{
			this.X += x;
			this.Y += y;
		}
		public void SetUp(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}
		public Vec2i GetCopy()
		{
			return new Vec2i(this.X, this.Y);
		}

		//Overload operatora
		public static Vec2i operator +(Vec2i V1, Vec2i V2) => new Vec2i(V1.X + V2.X, V1.Y + V2.Y);
		public static Vec2i operator -(Vec2i V1, Vec2i V2) => new Vec2i(V1.X - V2.X, V1.Y - V2.Y);
		public static Vec2i operator *(Vec2i V, int C) => new Vec2i(V.X * C, V.Y * C);
		public static Vec2i operator /(Vec2i V, int C) => new Vec2i(V.X / C, V.Y / C);
		public static bool operator ==(Vec2i V1, Vec2i V2) => V1.Equals(V2);
		public static bool operator !=(Vec2i V1, Vec2i V2) => !(V1 == V2);

		//Overrides od Base klase
		public override string ToString()
		{
			return $"V({this.X}, {this.Y})";
		}
		public override bool Equals(object obj)
		{
			return obj is Vec2i oVec ? (this.X == oVec.X && this.Y == oVec.Y) : (false);
		}
		public override int GetHashCode()
		{
			return this.X.GetHashCode() * 31 + this.Y.GetHashCode();
		}
	}
}
