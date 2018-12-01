class Needle {
  float posX, posY;
  float scaleX, scaleY;
  float strokeSize;

  float angleX_45, angleX_135, angleX_225, angleX_315;
  float angleY_45, angleY_135, angleY_225, angleY_315;

  Needle(float _posX, float _posY, float _scaleX, float _scaleY) {
    posX = _posX;
    posY = _posY;
    scaleX = _scaleX;
    scaleY = _scaleY;

    angleX_45 = (scaleX/2.0)*cos(1.0*PI/4.0);
    angleX_135 = (scaleX/2.0)*cos(3.0*PI/4.0);
    angleX_225 = (scaleX/2.0)*cos(5.0*PI/4.0);
    angleX_315 = (scaleX/2.0)*cos(7.0*PI/4.0);

    angleY_45 = (scaleY/2.0)*sin(1.0*PI/4.0);
    angleY_135 = (scaleY/2.0)*sin(3.0*PI/4.0);
    angleY_225 = (scaleY/2.0)*sin(5.0*PI/4.0);
    angleY_315 = (scaleY/2.0)*sin(7.0*PI/4.0);
  }

  void update() {
    display();
    action();
  }

  void display() {
    stroke(220, 20, 60);
    strokeWeight(2.0);
    line(posX, posY - scaleY/2.0, posX, posY + scaleY/2.0);
    line(posX - scaleX/2.0, posY, posX + scaleX/2.0, posY);
    line(posX + angleX_45, posY + angleY_45, posX + angleX_225, posY + angleY_225);
    line(posX + angleX_135, posY + angleY_135, posX + angleX_315, posY +angleY_315);

    fill(220, 20, 60);
    noStroke();
    ellipse(posX, posY, scaleX/1.4, scaleY/1.4);
  }

  void action() {
    if (abs(playerPosX - posX) < playerScaleX/2 + scaleX/2
      && abs(playerPosY - posY) < playerScaleY/2 + scaleY/2) {
      flagReset();
    }
  }
}

class MoveNeedle extends Needle {
  float startPosX, startPosY;
  float endPosX, endPosY;
  int lerpCount;
  int lerpCountSpan;

  ButtonBlock bb;

  MoveNeedle(float _startPosX, float _startPosY, float _scaleX, float _scaleY, 
    float _endPosX, float _endPosY, int _lerpCountSpan)
  {  
    super(_startPosX, _startPosY, _scaleX, _scaleY);

    startPosX = _startPosX;
    startPosY = _startPosY;
    endPosX = _endPosX;
    endPosY = _endPosY;
    lerpCountSpan = _lerpCountSpan;
  }

  void setTrap(float _buttonPosX, float _buttonPosY, float _buttonScaleX, float _buttonScaleY, Scene _scene) {
    bb = new ButtonBlock(_buttonPosX, _buttonPosY, _buttonScaleX, _buttonScaleY);
    bb.isTrap = true;

    _scene.addButtonBlock(bb);
  }
  void setButton(float _buttonPosX, float _buttonPosY, float _buttonScaleX, float _buttonScaleY, Scene _scene) {
    bb = new ButtonBlock(_buttonPosX, _buttonPosY, _buttonScaleX, _buttonScaleY);

    _scene.addButtonBlock(bb);
  }

  void update() {
    if (bb.action == true && lerpCount <= lerpCountSpan) {
      move();
    }
    super.update();
  }

  void move() {
    posX = (1.0 - lerpCount / (float)lerpCountSpan) * startPosX + (lerpCount / (float)lerpCountSpan) * endPosX;
    posY = (1.0 - lerpCount / (float)lerpCountSpan) * startPosY + (lerpCount / (float)lerpCountSpan) * endPosY;

    lerpCount++;
  }
}
