class Select {
  float posx;
  float posy;
  float wid;
  float hei;
  int id;
  String tex;
  color col;
  color cursorCol;

  int cursorColAl;
  boolean clear;
  PImage hanamaru;

  Select(float argX, float argY, float argW, float argH, int argId, String argText, color argColor, color argCursorColor) {
    posx = argX;
    posy = argY;
    wid = argW;
    hei = argH;
    id = argId;
    tex = argText;
    col = argColor;
    cursorCol = argCursorColor;
    cursorColAl = 200;
    clear = false;
    hanamaru = loadImage("images/hanamaru/hanamaru02.png");
  }

  void update() {   
    colider();
    display();
  }

  void display() {
    fill(col);
    rect(posx, posy, wid, hei);
    //クリア表示
    if (clear) {
      tint(220, 20, 60);
      image(hanamaru, posx, posy, wid+20, wid+20);
      noTint();
    }
    fill(255);
    textSize(26);
    text(tex, posx, posy);
  }

  void colider() {
    if ((abs(mouseX - posx) < wid/2) && (abs(mouseY - posy) < hei/2)) {
      cursorColAl -= 4;
      if (cursorColAl <= 80) {
        cursorColAl = 200;
      }
      selectCursor(posx, posy, wid, hei, cursorCol, cursorColAl);

      if (mouseReleased && id >= 0) {
        scene01 = true;
        scene02 = true;
        scene03 = true;
        scene04 = true;
        scene05 = true;

        switch(id) {
        case 0:
          scene = "SelectScene";
          break;
        case 1: 
          scene = "Stage01";
          break;
        case 2:
          scene = "Stage02";
          break;
        case 3:
          scene = "Stage03";
          break;
        case 4:
          scene = "Stage04";
          break;
        case 5:
          scene = "Stage05";
          break;
        case 101:
          player.guard = false;
          scene = "SelectScene";
          DeleteAndSet();
          break;
        case 102:
          scene = "SelectScene";
          break;
        case 103:
          exit();
          break;
        case 104:
          scene = "SelectScene";
          selectSet = true;
          break;
        case 110:
          if (money >= con(player.attack)) {
            money -= con(player.attack);
            player.attack++;
          }
          break;
        case 111:
          if (money >= con(player.shotNum)) {
            money -= con(player.shotNum);
            player.shotNum++;
          }
          break;
        default:
          scene = "SelectScene";
          break;
        }
      }
    } else {
      cursorColAl = 200;
    }
  }
}

class PowerUpButton extends Select {
  String compare;

  PowerUpButton(float argX, float argY, float argW, float argH, int argId, String argText, color argColor, color argCursorColor, String argCompare) {
    super(argX, argY, argW, argH, argId, argText, argColor, argCursorColor);
    compare = argCompare;
  }

  void update() {
    super.colider();
    this.display();
  }

  void display() {
    fill(col);
    rect(posx, posy, wid, hei);
    //クリア表示
    if (clear) {
      tint(220, 20, 60);
      image(hanamaru, posx, posy, wid+20, wid+20);
      noTint();
    }
    fill(255);
    textSize(26);
    if (compare == "ATTACK") {
      text(tex + "\n" + "$ " + str(con(player.attack)), posx, posy);
    } else if (compare == "ShotNum") {
      text(tex + "\n" + "$ " + str(con(player.shotNum)), posx, posy);
    }
  }
}

void selectCursor(float argX, float argY, float argWidth, float argHeight, color argColor, color argAlpha) {
  noFill();
  stroke(argColor, argAlpha);
  strokeWeight(10);
  if (mousePressed) {
    rect(argX, argY, argWidth+10, argHeight+10);
    fill(255, 100);
    stroke(0);
    noStroke();
    rect(argX, argY, argWidth, argHeight);
  } else {
    rect(argX, argY, argWidth+20, argHeight+20);
  }
  stroke(0);
  strokeWeight(1);
}

int con(int val) {
  int c = 0;
  c = int(10*val)+val*val;
  return c;
}