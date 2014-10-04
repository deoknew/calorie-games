#pragma strict
#pragma downcast
private
var _counter:float;
private
var _counterWait:float;
private
var _idle:boolean;
var names:String[];

function Start () {
names = GetAnimationNames(this.animation);
animation.Play("idleCasual");
}

function Update () {
	_counter+=Time.deltaTime;
	
	if(Input.GetKeyUp("1")){
		_counter = 0;
		animation.CrossFade(names[2]);
		_counterWait = 4;
		_idle = false;
	}else if(Input.GetKeyUp("2")){
		 animation.CrossFade("hit",.2);
		 _counter = 0;
		 _counterWait = 1;
		 _idle = false;
	}else if(Input.GetKeyUp("3")){
		animation.CrossFade("angry");
		_counter = 0;
		_counterWait = 5;
		_idle = false;
	}else if(Input.GetKeyUp("4")){
		animation.CrossFade("jawn");
		_counter = 0;
		_counterWait = 3;
		_idle = false;
	}else if(Input.GetKeyUp("5")){
		animation.CrossFade("smile");
		_counter = 0;
		_counterWait = 2;
		_idle = false;
	}else if(Input.GetKeyUp("6")){
		animation.CrossFade("death");
		_counter = 0;
		_counterWait = 3;
		_idle = false;
	}	
	
	if(_counter > _counterWait && !_idle){
		animation.CrossFade("idleCasual");
		_idle = true;
	}
}

function GetAnimationNames(anim:Animation){
	var tmpList : Array = new Array(); 
	for (var state : AnimationState in anim) {
		tmpList.Add(state.name);
	}
	var list : String[] = tmpList.ToBuiltin(String);
	return list;
}
