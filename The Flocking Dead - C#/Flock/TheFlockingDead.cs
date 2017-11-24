using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Flock;

/* 
 * the FlockingDead class
 */
public class TheFlockingDead : Form
{
	private Timer timer;
	private Swarm swarm;
	private Image iconRegular;
	private Image iconZombie;
    private bool scaryMouse = false;
    private Agent scaryMouseAgent;
    CheckBox scaryMouseToggleButton;
    Button addZombieButton;
    Button removeZombieButton;
    Button addHumanButton;
    Button removeHumanButton;
    private int boundary;

	[STAThread]
	private static void Main()
	{
		Application.Run(new TheFlockingDead());
	}

    //Sets the initial parameters
	public TheFlockingDead()
	{
        scaryMouseToggleButton = new CheckBox()
        {
            Location = new Point(10, 10),
            Size = new Size(100, 20),
            Text = "Scary mouse"
        };
        scaryMouseToggleButton.Click += new EventHandler(this.ButtonClick);

        addZombieButton = new Button()
        {
            Location = new Point(100, 10),
            Size = new Size(100, 20),
            Text = "Add zombie"
        };
        addZombieButton.Click += new EventHandler(this.ButtonClick);

        removeZombieButton = new Button()
        {
            Location = new Point(220, 10),
            Size = new Size(100, 20),
            Text = "Remove zombie",
        };
        removeZombieButton.Click += new EventHandler(this.ButtonClick);

        addHumanButton = new Button()
        {
            Location = new Point(340, 10),
            Size = new Size(100, 20),
            Text = "Add human"
        };
        addHumanButton.Click += new EventHandler(this.ButtonClick);

        removeHumanButton = new Button()
        {
            Location = new Point(460, 10),
            Size = new Size(100, 20),
            Text = "Remove human"
        };
        removeHumanButton.Click += new EventHandler(this.ButtonClick);


        Controls.Add(scaryMouseToggleButton);
        Controls.Add(addZombieButton);
        Controls.Add(removeZombieButton);
        Controls.Add(addHumanButton);
        Controls.Add(removeHumanButton);
		this.boundary = 640;
		SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
		FormBorderStyle = FormBorderStyle.FixedToolWindow;
		StartPosition = FormStartPosition.CenterScreen;
		ClientSize = new Size(boundary, boundary);
		iconRegular = CreateIcon(Brushes.Blue);
		iconZombie = CreateIcon(Brushes.Red);
		swarm = new Swarm(boundary);
		timer = new Timer();
		timer.Tick += new EventHandler(this.timer_Tick);
		timer.Interval = 2000/60;
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
        if(this.scaryMouse && this.scaryMouseAgent != null)
        {
            this.scaryMouseAgent.Position = this.PointToClient(Cursor.Position);
        }
		Invalidate();
	}

    private void ButtonClick(object sender, EventArgs e)
    {
        ButtonBase triggeredButton = (ButtonBase)sender;
        switch (triggeredButton.Text)
        {
            case "Scary mouse":
                this.scaryMouse = !this.scaryMouse;
                if (this.scaryMouse)
                {
                    //if we just toggled too true
                    this.scaryMouseAgent = new Agent(true, 0)
                    {
                        Position = this.PointToClient(Cursor.Position),
                        special = true
                    };
                    this.swarm.Agents.Add(scaryMouseAgent);
                }
                else
                {
                    this.swarm.Agents.Remove(scaryMouseAgent);
                    this.scaryMouseAgent = null;
                }
                break;
            case "Add zombie":
                this.swarm.SpawnNewAgent(true, this.boundary);
                break;
            case "Add human":
                this.swarm.SpawnNewAgent(false, this.boundary);
                break;
            case "Remove zombie":
                this.swarm.RemoveAgent(true);
                break;
            case "Remove human":
                this.swarm.RemoveAgent(false);
                break;
            default:
                Console.WriteLine(triggeredButton.Text + " has not yet been supported");
                break;
        }
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

    public void SpawnNewAgent(bool zombie, int boundary)
    {
        Agents.Add(new Agent(zombie, boundary));
    }

    public void RemoveAgent(bool zombie)
    {
        Agent agentToRemove = null;
        foreach(Agent a in Agents)
        {
            if(a.Zombie == zombie && !a.special)
            {
                agentToRemove = a;
                break;
            }
        }
        if(agentToRemove != null)
        {
            Agents.Remove(agentToRemove);
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
    private static float cohesionScalar = 0.2f;
    private static float alignmentPercentage = 0.5f;
    private static float seperationScalar = 0.1f;//Keep in mind that this also uses the space variable to trigger
    private static float evasionScalar = 1.2f;
    private static float huntScalar = 5f;
    private float boundary;
	public float dX;
	public float dY;
	public bool Zombie;
	public PointF Position;
    public bool special = false;

	public Agent(bool zombie, int boundary)
	{
		Position = new PointF(rnd.Next(boundary), rnd.Next(boundary));
		this.boundary = boundary;
		Zombie = zombie;
        //ensure the alignmentPercentage remains between -1 and 1
        alignmentPercentage = MathHelper.Clamp(alignmentPercentage, -1, 1);
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
					dX += (Position.X - a.Position.X) * seperationScalar;
					dY += (Position.Y - a.Position.Y) * seperationScalar;
				}
				else if (distance < sight)
				{
                    // Cohesion
                    dX += (a.Position.X - Position.X) * cohesionScalar;
                    dY += (a.Position.Y - Position.Y) * cohesionScalar;
                }
				if (distance < sight)
                {
                    // Alignment
                    dX += a.dX * alignmentPercentage;
                    dY += a.dY * alignmentPercentage;
                }
			}
			if (a.Zombie && distance < sight)
			{
                // Evade
                dX += (Position.X - a.Position.X) * evasionScalar;
                dY += (Position.Y - a.Position.Y) * evasionScalar;
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
            dX += (prey.Position.X - Position.X) * huntScalar;
            dY += (prey.Position.Y - Position.Y) * huntScalar;
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