using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

/* 
 * the FlockingDead class
 */
public class TheFlockingDead : Form
{
	private Timer timer;
	private Swarm swarm;
	private Image iconRegular;
	private Image iconZombie;

	[STAThread]
	private static void Main()
	{
		Application.Run(new TheFlockingDead());
	}

    //Sets the initial parameters
	public TheFlockingDead()
	{
		int boundary = 640;
		SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
		FormBorderStyle = FormBorderStyle.FixedToolWindow;
		StartPosition = FormStartPosition.CenterScreen;
		ClientSize = new Size(boundary, boundary);
		iconRegular = CreateIcon(Brushes.Blue);
		iconZombie = CreateIcon(Brushes.Red);
		swarm = new Swarm(boundary);
		timer = new Timer();
		timer.Tick += new EventHandler(this.timer_Tick);
		timer.Interval = 75;
		timer.Start();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		foreach (Agent agent in swarm.Agents)
		{
			float angle;
			if (agent.dX == 0) angle = 90f;
			else angle = (float)(Math.Atan(agent.dY / agent.dX) * 57.3);
			if (agent.dX < 0f) angle += 180f;
			Matrix matrix = new Matrix();
			matrix.RotateAt(angle, agent.Position);
			e.Graphics.Transform = matrix;
			if (agent.Zombie) e.Graphics.DrawImage(iconZombie, agent.Position);
			else e.Graphics.DrawImage(iconRegular, agent.Position);
		}
	}

	private static Image CreateIcon(Brush brush)
	{
		Bitmap icon = new Bitmap(16, 16);
		Graphics g = Graphics.FromImage(icon);
		Point p1 = new Point(0, 16);
		Point p2 = new Point(16, 8);
		Point p3 = new Point(0, 0);
		Point p4 = new Point(5, 8);
		Point[] points = { p1, p2, p3, p4 };
		g.FillPolygon(brush, points);
		return icon;
	}

	private void timer_Tick(object sender, EventArgs e)
	{
		swarm.MoveAgents();
		Invalidate();
	}
}

/*
 * the Swarm Class: creates new agents (including the zombies)
 */
public class Swarm
{
	public List<Agent> Agents = new List<Agent>();

	public Swarm(int boundary)
	{
		for (int i = 0; i < 15; i++)
		{
			Agents.Add(new Agent((i > 12), boundary));
		}
	}

	public void MoveAgents()
	{
		foreach (Agent a in Agents)
		{
			a.Move(Agents);
		}
	}
}

public class Agent
{
	private static Random rnd = new Random();
	private static float border = 100f;
	private static float sight = 75f;
	private static float space = 30f;
	private static float speed = 12f;
	private float boundary;
	public float dX;
	public float dY;
	public bool Zombie;
	public PointF Position;

	public Agent(bool zombie, int boundary)
	{
		Position = new PointF(rnd.Next(boundary), rnd.Next(boundary));
		this.boundary = boundary;
		Zombie = zombie;
	}

	public void Move(List<Agent> agents)
	{
        //Agents flock, zombie's hunt 
		if (!Zombie) Flock(agents);
		else Hunt(agents);
		CheckBounds();
		CheckSpeed();
		Position.X += dX;
		Position.Y += dY;
	}

	private void Flock(List<Agent> agents)
	{
		foreach (Agent a in agents)
		{
			float distance = Distance(Position, a.Position);
			if (a != this && !a.Zombie)
			{
				if (distance < space)
				{
					// Separation
					dX += Position.X - a.Position.X;
					dY += Position.Y - a.Position.Y;
				}
				else if (distance < sight)
				{
					// Cohesion
					//dX += TODO
					//dY += TODO
				}
				if (distance < sight)
				{
					// Alignment
					//dX += TODO
					//dY += TODO
				}
			}
			if (a.Zombie && distance < sight)
			{
				// Evade
				//dX += TODO
				//dY += TODO
			}
		}
	}

	private void Hunt(List<Agent> agents)
	{
		float range = float.MaxValue;
		Agent prey = null;
		foreach (Agent a in agents)
		{
			if (!a.Zombie)
			{
				float distance = Distance(Position, a.Position);
				if (distance < sight && distance < range)
				{
					range = distance;
					prey = a;
				}
			}
		}
		if (prey != null)
		{
            // Move towards prey.
            if (Position.X > prey.Position.X)
            {
                dX--;
            }else if(Position.X < prey.Position.X)
            {
                dX++;
            }

            if (Position.Y > prey.Position.Y)
            {
                dY--;
            }
            else if (Position.Y < prey.Position.Y)
            {
                dY++;
            }
        }
	}

	private static float Distance(PointF p1, PointF p2)
	{
		double val = Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2);
		return (float)Math.Sqrt(val);
	}

	private void CheckBounds()
	{
		float val = boundary - border;
		if (Position.X < border) dX += border - Position.X;
		if (Position.Y < border) dY += border - Position.Y;
		if (Position.X > val) dX += val - Position.X;
		if (Position.Y > val) dY += val - Position.Y;
	}

	private void CheckSpeed()
	{
		float s;
		if (!Zombie) s = speed;
		else s = speed / 4f; //Zombie's are slower
		float val = Distance(new PointF(0f, 0f), new PointF(dX, dY));
		if (val > s)
		{
			dX = dX * s / val;
			dY = dY * s / val;
		}
	}
}