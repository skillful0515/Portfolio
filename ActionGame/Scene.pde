class Scene {
  String sceneName = "hoge";

  ArrayList<Block> blocks;
  ArrayList<Door> doors;
  ArrayList<Needle> needles;
  ArrayList<MoveNeedle> moveNeedles;
  ArrayList<Save> saves;
  ArrayList<ButtonBlock> buttonBlocks;

  ArrayList<Shot> shots;
  ArrayList<Debris> debris;
  ArrayList<Boss> bosses;

  // シーン遷移後用変数
  boolean isChanging = false;
  int changingCount = 60;
  int changingCountSpan = 60;

  Scene(String _sceneName) {
    sceneName = _sceneName;

    blocks = new ArrayList<Block>();
    doors = new ArrayList<Door>();
    needles = new ArrayList<Needle>();
    moveNeedles = new ArrayList<MoveNeedle>();
    saves = new ArrayList<Save>();
    buttonBlocks = new ArrayList<ButtonBlock>();

    shots = new ArrayList<Shot>();
    debris = new ArrayList<Debris>();
    bosses = new ArrayList<Boss>();
  }

  void update() {
    for (Block b : blocks) {
      b.update();
    }
    for (Needle n : needles) {
      n.update();
    }
    for (MoveNeedle mn : moveNeedles) {
      mn.update();
    }

    for (Save s : saves) {
      s.update();
    }
    for (ButtonBlock b : buttonBlocks) {
      b.update();
    }

    for (Boss b : bosses) {
      b.update();
    }

    for (int i = bosses.size()-1; i >= 0; i--) {
      Boss b = bosses.get(i);
      if (b.del == true) {
        bosses.remove(i);
      }
    }

    for (Shot s : shots) {
      s.update();
    }

    for (Debris d : debris) {
      d.update();
    }


    // 暗転の関係上、最後にUpdateした方が良いかも
    for (Door d : doors) {
      d.update();
    }

    for (int i = shots.size()-1; i >= 0; i--) {
      Shot s = shots.get(i);
      if (s.del == true) {
        shots.remove(i);
      }
    }

    for (int i = debris.size()-1; i >= 0; i--) {
      Debris d = debris.get(i);
      if (d.del == true) {
        debris.remove(i);
      }
    }

    actChanging();
  }

  void actChanging() {
    if (isChanging == true) {
      cannotMove = true;
      changingCount--;

      noStroke();
      fill(0, (1.0 - (changingCountSpan - changingCount) / (float)changingCountSpan) * 255.0);
      rect(width/2, height/2, width, height);

      if (changingCount <= 0) {
        isChanging = false;
        changingCount = changingCountSpan;
        cannotMove = false;
        return;
      }
    }
  }

  void addBlock(Block _block) {
    blocks.add(_block);
  }

  void addDoor(Door _door) {
    doors.add(_door);
  }

  void addNeedle(Needle _needle) {
    needles.add(_needle);
  }

  void addMoveNeedle(MoveNeedle _moveNeedle) {
    moveNeedles.add(_moveNeedle);
  }

  void addSave(Save _save) {
    saves.add(_save);
  }

  void addButtonBlock(ButtonBlock _buttonBlock) {
    buttonBlocks.add(_buttonBlock);
  }

  void addShot(Shot _shot) {
    shots.add(_shot);
  }

  void addBoss(Boss _boss) {
    bosses.add(_boss);
  }

  void addDebris(Debris _debris) {
    debris.add(_debris);
  }
}
