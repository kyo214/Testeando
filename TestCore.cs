using Godot;
using RogueTest.Core;
using RogueTest.Core.Combat;
using RogueTest.Core.Entities;
using RogueTest.Core.Events;
using RogueTest.Core.Systems;
using RogueTest.Core.Weapons;
using System.Collections.Generic;
using System.Linq;

public partial class TestCore : Node
{
	private Game _game = null!;

	private readonly List<System.Numerics.Vector2> _spawnPositions =
	new()
	{
		new(200, 200),
		new(300, 200),
		new(400, 200)
	};
	private int _lastSpawnCount = 0;
	private int _lastWaveIndex = -1;
	private bool _damageTestDone = false;
	private bool _mapCompletePrinted = false;
	public override void _Ready()
	{
		GD.Print("==============================");
		GD.Print(" ROGUETEST AI TEST");
		GD.Print("==============================");

		_game = new Game();
		_game.Start();


		// =========================
		// PLAYER
		// =========================

		_game.Player.Position =
			new System.Numerics.Vector2(400, 400);


		GD.Print(
			$"Player HP: {_game.Player.Stats.Health}");



		// =========================
		// WEAPON
		// =========================

		WeaponDefinition weaponDefinition =
			new WeaponDefinition
			{
				Name = "Goblin Sword",
				Damage = 5,
				DamageType = DamageType.Physical,
				AttackCooldown = 1,
				AttackType = WeaponAttackType.Direct,
				Range = 50,
				TargetingMode = TargetingMode.Nearest
			};


		Weapon weapon =
			new Weapon
			{
				Name = weaponDefinition.Name,
				Damage = weaponDefinition.Damage,
				DamageType = weaponDefinition.DamageType,
				AttackCooldown = weaponDefinition.AttackCooldown,
				AttackType = weaponDefinition.AttackType,
				Range = weaponDefinition.Range,
				TargetingMode = weaponDefinition.TargetingMode
			};



		// =========================
		// ENEMY
		// =========================

		Enemy enemy =
			new Enemy
			{
				Name = "Goblin Test",
				Combat = _game.Combat,
				Weapon = weapon
			};


		enemy.Stats.MaxHealth = 100;
		enemy.Stats.MoveSpeed = 50;
		enemy.Stats.Damage = 10;

		enemy.AttackRange = 50;
		enemy.DetectionRange = 300;


		enemy.Position =
			new System.Numerics.Vector2(340, 400);


		_game.World.AddEntity(enemy);


		GD.Print(
			$"Enemy Spawned: {enemy.Name}");

		GD.Print(
			$"Distance: {(_game.Player.Position - enemy.Position).Length()}");


		GD.Print("==============================");
		GD.Print(" STARTING AI LOOP");
		GD.Print("==============================");
	}

	public override void _Process(double delta)
	{
		_game.Update(
			(float)delta);


		if (_game.World.Entities.Count > 1)
		{
			Enemy enemy =
				(Enemy)_game.World.Entities[1];


			if (Time.GetTicksMsec() % 500 < 20)
			{
				GD.Print(
					$"Enemy Pos: {enemy.Position} | " +
					$"Distance: {(_game.Player.Position - enemy.Position).Length()} | " +
					$"State: {enemy.AIState} | " +
					$"HP: {_game.Player.Stats.Health}");
			}
		}
	}
}
