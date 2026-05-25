//For reference:
//When using in unity, you should be able to just call the "talk_to_name" knots - those will branch out to relevant knots based on the global flags.
VAR player_name = "Mimi"
VAR player_friends_count = 0

VAR dayCount = 1
VAR actionCount = 0

//player attributes
VAR pragmatic = 0
VAR helpful = 0
VAR easygoing = 0
VAR ambitious = 0
VAR adventurous = 0
VAR friendly = 0
VAR thoughtful = 0
VAR funny=0
VAR creative=0

//change vals for attributes
VAR smallChange = 1
VAR medChange = 3
VAR bigChange = 5


//people met identifiers
VAR met_charlie = false
VAR met_emily = false
VAR met_beverly = false
VAR met_stevie = false
VAR met_annika = false
VAR met_lino = false
VAR met_coco = false
VAR met_korra = false
VAR met_august = false
VAR met_jojo = false

VAR stevie_already_friend=false

//dog quest
//step 0: talk to beverly for a description of where the dog might be
//step 1: find the dog and speak in dog language to get him to go home
//step 2: talk to beverly and confirm that coco made it home
//step 3: give charlie the good news
VAR dog_quest = false
VAR dog_step = 0 //0-3
VAR from_charlie=false
VAR char_convo = false


//emily-charlie quest
//step 0: talk to emily and find out she wants to connect with her son
//step 1: talk to stevie and buy the walkie-talkie (no currency, they start you a tab)
//step 2: talk to emily and give her the walkie talkie; convo with her and charlie
VAR emily_charlie_quest = false
VAR emily_charlie_step = 0 //0-2
VAR has_walkies = false


//lino gift quest
//step 0: talk to annika to get an idea of what she would like
//step 1: buy a gift from stevie (either a shawl, a fountain pen, or a chili dog)
//step 2: give the gift to lino
VAR lino_quest = false
VAR lino_step = 0 //0-2
VAR lino_gift = "" //options are null, shawl, pen, chili 


//annika settling in
//annika had her interview but is trying to decide if she wants to take the job.
//step 0: annika wants to know about public transit in the town - ask stevie for a bus schedule. progress by telling annika
//step 1: give annika the bus schedule; next she wants to know what neighborhood lino lives in. ask Lino. progress by telling annika
//step 2: give annika the neighborhood info, then find something happening in the city (talk to musician). progress by telling annika.
//step 3: give annika the information for the concert. progress by telling annika.
VAR annika_interviewed=false
VAR annika_quest=false
VAR annika_step=0 
VAR have_new_info_annika = false


//the four gophers
VAR four_gophers = false
VAR gophers_step = 0

//summer mural
VAR mural_quest = false
VAR mural_step = 0
VAR mural_loc = ""

//chrlie's pranks
VAR paint = false
VAR gum = false
VAR handshake = false

//friendship scores
VAR emily = 0
VAR charlie = 0
VAR beverly = 0
VAR stevie = 0
VAR stevieSales=0
VAR lino = 0
VAR coco = 0
VAR annika = 0
VAR korra = 0
VAR august = 0
VAR jojo = 0

EXTERNAL add_friend(name)

-> talk_to_someone

=== talk_to_someone ===
+ Charlie
    ->talk_to_charlie
+ Emily
    ->talk_to_emily
+ Beverly
    ->talk_to_beverly
+ Shopkeeper
    ->talk_to_shopkeeper
+ Coco
    ->talk_to_coco
+ Lino
    ->talk_to_lino
+ August
    ->talk_to_august
+ Jojo 
    ->talk_to_jojo


/////////////////////////////////////////
//dog quests////////////////////////////
/////////////////////////////////////////
===epilogue===
My first week in town has been such a fun one!
//Charlie and emily, or charlie and beverly
{dog_step==4:
    I've gotten to dogsit for Beverly a couple of times so far. Charlie and I even walked him down to the park together. It turns out that Coco really likes to explore the city - and Charlie's mom is okay with him exploring as long as he's under a watchful eye!
}
{mural_loc=="empty wall":
    August revamped the empty wall near Beverly's house, and it couldn't have turned out any better! It's practical, of course - a stylized map of town so that no more newcomers get lost. But the colors and details really demonstrate everyone's love for the community. If you look hard, you can find a miniature Coco, Charlie and Jojo and the other kids running around the town, the local stage, it seems everyone is represented here. There's even a little drawing of me running errands around town.
}
{emily_charlie_step==3:
    Emily and Charlie have been having a great time exploring town together. I guess Emily grew up here in the '90s; she's showing him all her old hangout spots. The walikie-talkies really seem to hav e brought her back to the good old days.
}
{mural_loc=="school":
    After Charlie's flag football game, Emily and I took some photos in front of the mural that August painted, featuring handprints of everyone from around town. I even got to put my own handprint amongst the others - I guess I'm officially part of the town now!
}
{jojo==3:
    Jojo is still playing pranks on me, but I've gotten him with a few myself. I forgot how fun some of those practical jokes were - it was a great excuse to break out my old whoopie cushion and can of snakes from my childhood toys! 
}
{stevie>=5:
    Stevie has started a new venture... lawn gnomes. Of course, I had to get one for my front door, but he's somehow talked me into getting several more. I've got a collection going now. It's a bit strange, but honestly, they're pretty cute!
}
{lino_step==3:
    {lino_gift=="pen":
        I guess Lino's cousin really liked the pen - she's been carrying it nonstop! And, of course, Lino is still overjoyed that his cousin is in town.
    -else:
        {lino_gift=="shawl":
            I guess Lino's cousin really liked the shawl - she seems to be wearing it everywhere! And, of course, Lino is still overjoyed that his cousin is in town.
        }
    }
}
{annika_step==4:
    Annika decided to take the job! She's still in training, but she seems to be enjoying the work she's doing. And she's keeping busy outside of work, too - she's already found a couple of clubs and seems to have really settled in!
}
{gophers_step==3:
    I went to see the Four Gophers perform Friday night. I didn't expect it to be my kind of music, but I really had a great time! Korra and Annika have a lot of musical talent, and it was so nice to see them let loose on stage.
    {mural_loc=="park":
      August's mural gave the stage a nice touch, too - he painted all four musicians (as gophers, of course!) in all their glory. He's even making them a matching album cover for their first release!  
    }
}
I'm still settling in, but I'm grateful to have made so many friends and I know it won't be long before I feel at home here.
->END
===action_check===
{dayCount==1 or dayCount==2:
    {actionCount==2:
        ~dayCount+=1
        ~actionCount=0
        ->END
    -else:
        ->END
    }
-else:
    {dayCount==3:
        {actionCount==3:
            ~dayCount+=1
            ~actionCount=0
            ->action_check
        -else:
            ->END
        }
    -else: //day is 4 - epilogue
        ->epilogue
    }
}

->END
=== talk_to_coco===
{met_coco==false:
    ->coco_meet_convo
-else:
    {dog_step<=1:
        ->coco_come_home
    -else:
        ->coco_chat
    }
}
=== talk_to_charlie ===
{met_charlie:
    {dog_step==3:
        ->charlie_dog_news
    -else:
        {dog_step==4:
        ->charlie_post_dog
        -else:
        {emily_charlie_step==3:
        ->charlie_post_walkies
        -else:
        ->charlie_general_chat
        }
        }
    }
-else:
->charlie_meet_convo
}
===charlie_dog_news===
#speaker: Charlie
Hey! You came back!
+ You're still up there?
    #speaker: Charlie
    What do you mean? Of course I'm still up here.
    #speaker: Charlie
    I'm grounded for a whole week, you know.
    #speaker: Charlie
    Come back once you have good news.
    ~charlie-=1
    ->END
+ Yes, and I have good news.
    #speaker: Charlie
    You found Coco?
    #speaker: Charlie
    That's great news!
    #speaker: Charlie
    Whew, now that that's taken care of, I can figure out how to get out of my grounding situation...
    #speaker: Narrator
    Quest: The Lost Dog
    #speaker: Narrator
    Quest Complete!
    ~actionCount+=1
    ~ add_friend("charlie")
    ~charlie+=5
    ~dog_step=4
    ~dog_quest=false
    ~helpful+=medChange
    ~pragmatic+=medChange
    ~friendly+=medChange
    ->action_check
    
+ I found Coco!
    #speaker: Charlie
    Yes!!! I knew we could do it!
    #speaker: Charlie
    I bet Beverly is glad.
    #speaker: Charlie
    Ah - don't let my mom know I called her by her first name!
    #speaker: Narrator
    Quest: The Lost Dog
    #speaker: Narrator
    Quest Complete!
    ~actionCount+=1
    ~ add_friend("charlie")
    ~charlie+=5
    ~dog_step=4
    ~dog_quest=false
    ~helpful+=medChange
    ~adventurous+=medChange
    ~friendly+=medChange
    ->action_check
+ Got to go, we'll talk soon.
    ->END
===talk_to_beverly===
{met_beverly:
    {dog_quest:
        ->beverly_quest_progress
    -else:
        {dog_step==3:
            ->beverly_chat
        -else:
            ->beverly_chat
        }
    }
-else:
    ->beverly_meet_convo
}
===beverly_quest_start===
#speaker: Beverly
Oh, dear. Coco, Coco, please come back!
+ Coco?
    #speaker: Beverly
    Oh?
    #speaker: Beverly
    Oh, I'm sorry, dear. Coco is my sweet dog.
    #speaker: Beverly
    She hasn't come back in a while.
    ->beverly_quest_offer
+ Oh no, is everything alright?
    #speaker: Beverly
    Truthfully, not at all.
    #speaker: Beverly
    My sweet dog Coco wandered off yesterday afternoon and hasn't returned.
    #speaker: Beverly
    I'm worried she is lost, but I can't walk very far on my own.
    ->beverly_quest_offer
+ Keep Walking.
    ->END
===beverly_quest_offer===
+ How can I help?
    #speaker: Beverly
    You'd help me? Thank you, I am so grateful!
    ~dog_quest=true
    ~dog_step=0
    ~beverly+=1
    ~coco+=1
    ~friendly+=smallChange
    ~helpful+=smallChange
    ~adventurous+=smallChange
    ->beverly_quest_progress
+ That's too bad, I'm sorry to hear it. Best of luck.
    #speaker: Beverly
    Thank you.
    ~beverly -=1
    ~coco -=1
    ~friendly-=smallChange
    ~helpful-=smallChange
    ->END
+ I don't have time for this.
    ~beverly -=5
    ~coco -=5
    ~friendly-=smallChange
    ~helpful-=smallChange
    ->END
===beverly_quest_progress===
* {dog_step==0 and dog_quest==true} [Can you give me a description of your lost dog?]
    #speaker: Beverly
    Hmm? Coco?
    #speaker: Beverly
    Does that mean you'll help me look? Thank you, dear.
    #speaker: Beverly
    He is fluffy, gray, and about knee-height.
    #speaker: Beverly
    He prefers to answer to "Wuf-Wuf" rather than "Woof-Woof". I think he still has an accent from the southern region.
    #speaker: Beverly
    He doesn't like to stray far; he should still be in the neighborhood somewhere.
    #speaker: Narrator
    Quest: The Lost Dog
    #speaker: Narrator
    Next Objective: Find Coco
    ~dog_step=1
    ~pragmatic+=smallChange
    ~helpful+=smallChange
    ~beverly+=1
    ->END
* {dog_step==0 and from_charlie} [Charlie told me your dog is lost, I am so sorry! How can I help?]
    #speaker: Beverly
    Thank you, dear. I'm really beside myself.
    #speaker: Beverly
    It was so helpful of Charlie to help me look. Coco adores him.
    #speaker: Beverly
    If you wouldn't mind helping me look for Coco...
    #speaker: Beverly
    He is fluffy, gray, and about knee-height.
    #speaker: Beverly
    He prefers to answer to "Wuf-Wuf" rather than "Woof-Woof". I think he still has an accent from the southern region.
    #speaker: Beverly
    Thank you for your help, dear.
    #speaker: Narrator
    Quest: The Lost Dog
    #speaker: Narrator
    Next Objective: Find Coco
    ~dog_step=1
    ~helpful+=smallChange
    ~thoughtful+=smallChange
    ~beverly+=1
    ->END
* {dog_step==2 and met_coco}[I think I saw Coco down the street!]
    #speaker: Beverly
    What? That's great news!
    #speaker: Beverly
    It's too far for me to walk, though.
    #speaker: Beverly
    Could you please tell him to come home?
    #speaker: Beverly
    He prefers to answer to "Wuf-Wuf" rather than "Woof-Woof". I think he still has an accent from the southern region.
    ->END
* {dog_step==0} [I'm busy now, we'll talk later.]
    ->END
* {dog_step==1} [Can you remind me what Coco looks like?]
    #speaker: Beverly
    Of course, dear.
    #speaker: Beverly
    He is fluffy, gray, and about knee-height.
    #speaker: Beverly
    He prefers to answer to "Wuf-Wuf" rather than "Woof-Woof". I think he still has an accent from the southern region.
    #speaker: Beverly
    Thank you for your help, dear.
    ->END
* {dog_step==1} [I think I have what I need, I'll talk to you later.]
    ->END
* {dog_step==2}[Hi, Beverly! I think I found Coco. Did he make it home okay?]
    #speaker: Beverly
    Yes, he ran home just a minute ago!
    #speaker: Beverly
    I think he's napping now - I gave him a whole tray of biscuits as a reward.
    #speaker: Narrator
    That sounds like too many biscuits for a dog...
    #speaker: Narrator
    Still, I'm glad he made it home.
    #speaker: Beverly
    I can't tell you how grateful I am. If you ever need a favor, you know where to find me.
    #speaker: Beverly
    Oh, would you tell Charlie that Coco came home?
    #speaker: Beverly
    I'm sure he'd be relieved to hear it!
    #speaker: Narrator
    Quest: The Lost Dog
    #speaker: Narrator
    Next Objective: Check in with Charlie
    ~ add_friend("beverly")
    ~dog_step=3
    ~helpful+=medChange
    ~adventurous+=smallChange
    ~beverly+=5
    ~coco+=5
    ->END
    
===beverly_chat===
#speaker: Beverly
Ah, {player_name}, it's lovely to see you, dear!
#speaker: Beverly
Do you need help with anything?
* {mural_step==0 and mural_quest}[Actually, yes! Do you know anywhere in town where August could paint their next mural?]
    #speaker: Beverly
    August is looking for somewhere to paint their next mural?
    #speaker: Beverly
    That's excellent news! Perhaps they could paint something on this free wall here?
    #speaker: Beverly
    It would certainly liven up this street.
    #speaker: Beverly
    And, of course, I would love the chance to catch up with August.
    #speaker: Beverly
    We used to be schoolmates, you know. They were not always the best at arithmetic or spelling, but their art has always been astonishing.
    #speaker: Beverly
    I look forward to seeing what they'll paint next!
    #speaker: Narrator
    Quest: A Summer Mural
    #speaker: Narrator
    Next Objective: Tell August where they can paint their next mural.
    ~mural_step=1
    ~mural_loc="empty wall"
    ->END
+ No, thank you!
    #speaker: Beverly
    Well, you know where to find me if you need anything, dear!
    ->END
===coco_meet_convo===
#speaker: Unknown
Wuf, Wuf!
#speaker: Narrator
A strange gray dog looks at you, wagging his tail.
#speaker: Narrator
You think he's trying to tell you something.
~met_coco=true
->END
===coco_come_home===
#speaker: Coco
Wuf-Wuf!
#speaker: Narrator
Coco looks like he wants to talk with you.
#speaker: Narrator
Maybe you can tell him to come home?
+ Woof! Woof!
    #speaker: Coco
    Wuf. Grrrr.
    #speaker: Narrator
    Coco's ears flatten and he sits back, annoyed.
    #speaker: Narrator
    Whatever you said, it wasn't very polite!
    ->END
+ Bow-wow!
    #speaker: Coco
    Wuf. 
    #speaker: Narrator
    Coco shakes his head side-to-side.
    #speaker: Narrator
    He doesn't seem to understand you.
    ->END
* {dog_step==1}[Wuf-Wuf!]
    #speaker: Coco
    Wuf? 
    #speaker: Narrator
    Coco tips his head to the side.
    #speaker: Coco
    Wuf! Wuf-Wuf!
    #speaker: Narrator
    It seems Coco can understand you!
    #speaker: Narrator
    Coco runs back home.
    #speaker: Narrator
    You should probably see if he made it back okay.
    #speaker: Narrator
    Quest: The Lost Dog
    #speaker: Narrator
    Next Objective: Check in with Beverly
    ~ add_friend("coco")
    ~friendly+=smallChange
    ~adventurous+=smallChange
    ~coco+=5
    ~dog_step=2
    ->END
+ Scratch Coco behind the ears.
    #speaker: Narrator
    Coco leans into your hand and gives a big shake.
    ->END
* {dog_step>=0} [Try to pull Coco back home.]
    #speaker: Coco
    Grrrrrr.....
    #speaker: Narrator
    Coco is heavier than you expected. You can't pull her home.
    ~coco -=1
    ~friendly-=smallChange
    ->END
===coco_chat===
#speaker: Coco
Wuf!
+ Pet Coco
    #speaker: Coco
    Wuf! Wuf-Wuf-Wuf!
    #speaker: Narrator
    Coco wags her tail happily.
    ->END
+ Woof!
    #speaker: Coco
    Wuf.
    ->END
=== beverly_meet_convo ===
#speaker: Unknown
Oh... dear me...
+ Hello!
    #speaker: Unknown
    I'm sorry, hello.
    #speaker: Unknown
    I'm a little busy at the moment.
    #speaker: Unknown
    My name is Beverly, if you need anything.
    ~met_beverly=true
    -> END
*{dog_quest} [Excuse me, are you Beverly?]
    #speaker: Unknown
    Hmm?
    #speaker: Unknown
    Oh, yes. You were looking for me?
    ~met_beverly=true
    -> talk_to_beverly
+ Keep Walking.
    ->END
=== charlie_day_one ===

{ met_charlie:

-> talk_to_charlie

-else: 

-> charlie_meet_convo
}
=== charlie_meet_convo ===
#speaker: Unknown
Hey, lady! 
#speaker: Narrator
You look for the voice. You don’t see anyone in the street.
#speaker: Unknown
I’m up here!
#speaker: Narrator
He appears to be much younger than you - maybe 12 years old or so.
#speaker: Unknown
Took you long enough. My name is Charlie!
#speaker: Player
Oh, hi, Charlie.
~ met_charlie = true
-> charlie_first_convo


===charlie_general_chat===
#speaker: Charlie
Hey there! Back so soon?
* {dog_quest==false and emily_charlie_quest==false and dog_step==0 and emily_charlie_step==0 and dayCount==1}[You said you needed help with something?]
    ->lost_dog_overview
+ Just stopping by, we can talk later.
    ->END

=== charlie_first_convo ===

* {dayCount==1}[What are you doing up there?]
    ~char_convo=true
    ->lost_dog_overview

+ Do I know you?
    #speaker: Charlie
    Nope! I don’t think so? Unless you’re friends with my mom?
    #speaker: Player
    No, I’m new to the area.
    #speaker: Charlie
    Well then I guess I wouldn’t know you, then.
    #speaker: Player
    Hmm, I guess not.
    ->charlie_first_convo


+ [Keep Walking]
    ->END
-> END
=== lost_dog_overview ===
#speaker: Charlie
Hmm? Oh. Right.
#speaker: Narrator
Charlie sighs
#speaker: Charlie
I was grounded for staying out too late last night.
#speaker: Charlie
But it’s not my fault!
#speaker: Charlie
My neighbor’s dog got loose and I was just trying to track it down…
#speaker: Charlie
I would have stayed out later than that. But mom was already so mad…
#speaker: Charlie
She wouldn’t even listen to my reason for staying out!

+  That’s not fair at all. Maybe I can talk to your mom and explain the situation to her.
    ~ ambitious += 1
    ~ easygoing += 1
    ~ charlie += 1
    ~ emily+= 1
    ~ emily_charlie_quest = true
    #speaker: Charlie
    You would do that for me?
    #speaker: Charlie
    Hmm, maybe she *would* listen to another adult…
    #speaker: Charlie
    Well, thanks! You should be able to find her on the street somewhere. 
    #speaker: Charlie
    I think her name is Emily? I’m not supposed to call her that, though…
    #speaker: Narrator
    Quest started: Unite Emily and Charlie
    #speaker: Narrator
    Next Objective: Find Emily
    -> END
+  Oh no, your neighbor’s dog is missing? I can help look for it.
    ~ adventurous +=1
    ~ helpful +=1
    ~ dog_quest = true
    ~from_charlie=true
    ~ charlie += 1
    ~ beverly += 1
    #speaker: Charlie
    Thank you! He’s gray and fluffy and should still be in the neighborhood somewhere.
    #speaker: Charlie
    My neighbor who lost the dog lives down the street. My mom calls her Beverly.
    #speaker: Charlie
    She should be able to tell you more.
    
    #speaker: Narrator
    Quest started: The Lost Dog
    #speaker: Narrator
    Next Objective Find Beverly
    -> END

+ That’s too bad. Good luck with that, I hope everything is okay.
    ~ helpful -= 1
    ~ charlie -= 1
    ~ beverly -= 1
    #speaker: Charlie
    Thanks anyway.
    -> END
===charlie_post_dog===
#speaker: Charlie
Hey! What are you up to
* {mural_step==0 and mural_quest==true}[I'm trying to find somewhere for August to paint their next mural. Any ideas?]
    #speaker: Charlie
    Oh, that's an easy one!
    #speaker: Charlie
    The school has been looking for some new art for ages. 
    #speaker: Charlie
    Mom's on the school board so I hear all the drama. 
    #speaker: Charlie
    But, yeah, they should be good to paint there.
    #speaker: Narrator
    Quest: A Summer Mural
    #speaker: Narrator
    Next Objective: Tell August where they can paint their next mural.
    ~mural_step=1
    ~mural_loc="school"
    ->END
+ Not much, still grounded?
    #speaker: Charlie
    Ugh. Yes, another 6 days, I think.
    #speaker: Charlie
    Wish mom would just let me out already...
    ->END
+ Errands upon errands...
    #speaker: Charlie
    Oof. I'd take grounding over that any day...
    ->END
    
    
    
===charlie_post_walkies===
#speaker: Charlie
Hey! I'm not grounded anymore!
* {mural_step == 1} [That's great news! Any chance you can help me find where August can paint a mural?]
    #speaker: Charlie
    Oh, that's an easy one!
    #speaker: Charlie
    The school has been looking for some new art for ages. 
    #speaker: Charlie
    Mom's on the school board so I hear all the drama. 
    #speaker: Charlie
    But, yeah, they should be good to paint there.
    #speaker: Narrator
    Quest: A Summer Mural
    #speaker: Narrator
    Next Objective: Tell August where they can paint their next mural.
    ~mural_step=1
    ~mural_loc="school"
    ->END
* {mural_step != 1} [Excellent!]
    ~ add_friend("charlie")
    #speaker: Charlie
    Thanks for talking to my mom.
    #speaker: Charlie
    I'm allowed to explore even more now that I've got this walkie-talkie!
    ->END
+ Stay out of trouble!
    #speaker: Charlie
    Will do!
    ->END
    
===talk_to_emily===
////////////////////////////////////////
//emily-charlie quests///////////////////
/////////////////////////////////////////
{met_emily:
    {emily_charlie_step==3:
        ->emily_post_walkies
    -else:
        {emily_charlie_quest:
            ->emily_walkies
        -else:
            ->emily_chat
        }
    }
-else:
->emily_meet_convo
}
===talk_to_shopkeeper===
{met_stevie:
-> stevie_quest_convo
-else:
->stevie_first_convo
}
=== emily_day_one ===
{emily_charlie_quest:
    {met_emily:
        {emily_charlie_step==0 and dayCount==1:
            ->emily_charlie_questintro
        -else:
            {emily_charlie_step==1:
                ->emily_charlie_step1
            -else:
                ->emily_chat
            }
        }
    -else:
    -> emily_meet_convo
    }
-else:
    #narrative She looks too busy to speak with you.
    -> END
}
=== emily_meet_convo ===
#speaker: Unknown
Mph. 
#speaker: Unknown
Kids these days...
* {emily_charlie_quest}[Are you Emily?]
    ->emily_charlie_overview
+ She looks busy. Keep Walking.
    ->END

===emily_chat===
#speaker: Emily
Hello.
#speaker: Emily
I'm sorry, I'm a bit busy right now. Maybe we can talk later...
->END

=== emily_charlie_overview ===
#speaker: Unknown
Hmm?
#speaker: Emily
Oh! Yes, I'm Emily. And you are...?
#speaker: Player
I'm {player_name}.
~met_emily = true
{emily_charlie_quest and dayCount==1:
    ->emily_charlie_questintro
-else: 
    Good to meet you.
    ->END
}

===emily_charlie_questintro===
+ Your son Charlie asked me to speak with you.
    #speaker: Emily
    Ugh, I should have known he'd try to bother a stranger.
    #speaker: Emily
    Not sure why I ever gave him the room with a window...
    #speaker: Narrator
    Emily sighs.
    -> emily_charlie_reaction
+ Did you hear that Beverly's dog is missing?
    ~ambitious+=smallChange
    ~friendly+=smallChange
    #speaker: Emily
    Oh no, how awful!
    #speaker: Emily
    Though, now that you mention it, Charlie did say something about a lost dog.
    #speaker: Emily
    Maybe he was helping Beverly look for it?
    #speaker: Emily
    If that's the case, then I feel awful for grounding him...
    #speaker: Emily
    As much as I'd like him to help Beverly, though, I need to be able to check in on him.
    #speaker: Emily
    The world is a scary place, after all.
    #speaker: Emily
    Hmm, I have a thought...
    ->emily_charlie_step1
-> END
===emily_charlie_reaction===
#speaker: Emily
I'm sorry if he bothered you.
+ Not at all! He's actually trying to find his neighbor's lost dog
    ~friendly+=smallChange
    ~helpful+=smallChange
    #speaker: Emily
    Really? That's so sweet of him.
    #speaker: Emily
    I feel awful for grounding him, then.
    #speaker: Emily
    But...
    #speaker: Emily
    I still need to know where he's at.
    #speaker: Emily
    The world is a scary place after all...
    ->emily_charlie_step1
+ Thank you, you should really keep an eye on him, though.
    ~friendly-=smallChange
    ~thoughtful-=smallChange
    #speaker: Emily
    You're right.
    #speaker: Emily
    I try, I really do.
    #speaker: Emily
    But since I'm working so often, it's hard to know what he's doing.
    #speaker: Emily
    And I can never find him when I need him
    #speaker: Emily.
    I'd like for him to be able to explore,
    #speaker: Emily
    But I need to be able to check in on him.
    ->emily_charlie_step1
    
=== emily_charlie_step1 ===
#speaker: Emily
You're a young person. You probably have a better idea of these things.
#speaker: Emily
Do you think you can find a way for me to stay in touch with my son?
#speaker: Emily
That way he can help Beverly look for her dog, and I can contact him when I need to check-in.
~emily_charlie_step=1
#speaker: Narrator
Quest: Emily and Charlie
#speaker: Narrator
Next Objective: Find Communication Device
->END



//Stevie convos
===emily_walkies===
#speaker: Emily
You came back; that was fast.
#speaker: Emily
Any news to share?
* {has_walkies} [Give Emily the Walkie-Talkies]
    #speaker: Emily
    Oh my goodness!
    #speaker: Emily
    These are just like the ones back from when I was a girl. 
    #speaker: Narrator
    Emily holds down the button on the transmitter.
    #speaker: Emily
    testing, testing, one, two, three!
    #speaker: Narrator
    The receiver crackles back
    #speaker: Unknown
    ...testing...testing...one...two...three
    #speaker: Emily
    Oh, these will be so much fun!
    #speaker: Emily
    Charlie and I might even search the neighborhood together with these.
    #speaker: Emily
    Thank you, {player_name}, I can't tell you how much I appreciate your help!
    #speaker: Narrator
    Quest: Emily and Charlie
    #speaker: Narrator
    Quest Complete!
    ~actionCount+=1
    ~ add_friend("emily")
    ~emily_charlie_step=3
    ~emily_charlie_quest=false
    ~has_walkies=false
    ~emily+=5
    ~charlie+=5
    ~pragmatic+=medChange
    ~helpful+=bigChange
    ->action_check
    
+ Not yet, I'll come back later.
    #speaker: Emily
    Okay, please don't take too long.
    ->END
===emily_post_walkies===
#speaker: Emily
Hello, {player_name}!
#speaker: Emily
Thanks again for your help earlier; I'm having so much fun with these walkie-talkies.
#speaker: Emily
I think Charlie might be getting a little sick of them!
#speaker: Charlie
...roger, mom. don't hold down the button when you're not transmitting...
+ It sounds like he's enjoying them, too!
    #speaker: Charlie
    ...roger!...
    #speaker: Charlie
    ...these are really cool...
    #speaker: Narrator
    Emily laughs and releases the button
    #speaker: Emily
    I can't remember the last time the two of us have played together like this.
    #speaker: Emily
    Thank you so much!
    ->END
+ Glad you're having fun!
    ->END

===talk_to_lino===
////////////////////////////////////////
//lino quests///////////////////
/////////////////////////////////////////
{met_lino:
    {lino_quest:
        {lino_step<=2:
            ->lino_ask_qs
        -else:
            {lino_step==3:
                ->lino_give_base
            -else:
                ->lino_chat
            }
        }
    -else:
    {dayCount==1 and lino_step==0:
        ->lino_quest_start
    -else:
        ->lino_chat
    }
    }
-else:
->lino_meet_convo
}

===lino_ask_qs===
+ Can you remind me of your cousin's name?
    #speaker: Lino
    Of course. Her name is Annika. She should be in this neighborhood somewhere.
    ->END
+ Do I really need to do this quest?
    #speaker: Lino
    No, I suppose not...
    #speaker: Lino
    That's quite rude to back out of your offer, though.
    #speaker: Narrator
    You hand back Lino's money.
    ~lino -=1
    ~friendly-=smallChange
    ~thoughtful-=smallChange
    ->END
+ Talk to you later.
    ->END
    
===lino_give_base===
    #speaker: Lino
Hello, {player_name}!
Did you find something?
* {lino_gift=="pen"}[Give Lino the fountain pen]
    #speaker: Lino
    Stunning! Annika is going to love this.
    #speaker: Lino
    She has a job interview coming up. This will be just the confidence boost she needs.
    #speaker: Lino
    Thank you for finding this. I really do appreciate it.
    ~lino+=5
    ~helpful+=bigChange
    ~pragmatic+=bigChange
    ~lino_step=4
    ~lino_quest=false
    #speaker: Narrator
    Quest: A Gift for Annika
    #speaker: Narrator
    Quest Complete!
    ~ add_friend("lino")
    ~actionCount+=1
    ->action_check
    ->END
* {lino_gift=="shawl"}[Give Lino the shawl]
    #speaker: Lino
    Oh, incredible.
    #speaker: Narrator
    Lino accepts the soft bundle of fabric.
    #speaker: Lino
    It's so soft and warm. And it looks just like something she used to wear at home.
    #speaker: Lino
    Thank you, truly. She is going to love this.
    ~ add_friend("lino")
    ~lino+=5
    ~helpful+=bigChange
    ~thoughtful+=bigChange
    ~lino_step=4
    ~lino_quest=false
    #speaker: Narrator
    Quest: A Gift for Annika
    #speaker: Narrator
    Quest Complete!
    ~actionCount+=1
    ->action_check
* {lino_gift=="chili"}[Give Lino the chili dog]
    #speaker: Lino
    Eh... what is that?
    #speaker: Narrator
    Lino accepts the foil-wrapped chili dog and sniffs it.
    #speaker: Narrator
    Lino retches
    #speaker: Lino
    Ugh, where did you find this?
    #speaker: Lino
    I don't have any money left for another gift... I suppose this will have to do...
    // ~lino-=5
    ~thoughtful-=smallChange
    ~funny+=smallChange
    ~lino_quest=false
    ~lino_step=4
    #speaker: Narrator
    Quest: A Gift for Annika
    #speaker: Narrator
    Quest Complete!
    ~actionCount+=1
    ->action_check
+ Not yet. I'll let you know as soon as I do.
    ->END
    
    
===lino_chat===
{lino>=0:
    ->lino_chat_positive
-else: 
    ->lino_chat_negative
}
===lino_chat_positive===
#speaker: Lino
Hello, my friend!
#speaker: Lino
How are you? What can I do for you?
* {annika_step==1}[Lino, can you remind me which neighborhood you live in?]
    #speaker: Lino
    Ah, yes, of course!
    #speaker: Lino
    I am over on the east side of town.
    #speaker: Lino
    Formally, it's called Reagent Village...
    #speaker: Lino
    But we just call it the Orange Grove!
    ~have_new_info_annika=true
    ->END
+ I'm alright at the moment, but thank you!
    #speaker: Lino
    Alright, we will talk again soon!
    ->END

===lino_chat_negative===
#speaker: Lino
I don't really want to talk to you right now...
#speaker: Lino
I think my sister is sick now...
->END

===lino_quest_start===
+ You look like you need help with something.
    #speaker: Lino
    That's okay, but...
    #speaker: Lino
    Hmm. Actually...
    #speaker: Narrator
    Lino pauses and scratches his chin.
    #speaker: Lino
    Maybe you could help me after all.
    #speaker: Lino
    I'm trying to find a gift for my cousin.
    #speaker: Lino
    You see, she just moved to the area.
    #speaker: Lino
    I'd love to get her a gift to welcome her to the city...
    #speaker: Narrator
    Lino sighs.
    #speaker: Lino
    But I'm terrible with gifts.
    #speaker: Lino
    You, though - you seem to be about her age.
    #speaker: Lino
    Could you pick out a gift for me?
    #speaker: Narrator
    Before you have a chance to respond, Lino presses the money into your hand.
    #speaker: Lino
    Her name is Annika. She is a strong, smart girl and she deserves the best!
    #speaker: Lino
    Please bring me whatever you find for her.
    ~lino_quest=true
    ~lino_step=0
    #speaker: Narrator
    Quest: A Gift for Annika
    #speaker: Narrator
    Next Objective: Ask Annika what she likes
    ->END
    
+ What are you muttering about?
    #speaker: Lino
    Ah, I was talking out loud, wasn't I?
    I can't help it. I'm sorry if I bothered you.
    ->END

+ My mistake, I thought you were someone else.
    #speaker: Lino
    No worries.
    ->END
===lino_meet_convo===
#speaker: Unknown
Hmm, not that.
#speaker: Unknown
Not that one, either.
+ Hello there!
    #speaker: Unknown
    Oh! Hello!
    #speaker: Unknown
    My name is Lino. 
    #speaker: Lino
    ~met_lino=true
    ->lino_quest_start
+ Keep Walking.
    ->END

===lino_checkin===
* {dayCount==1}[Can I help you with anything?]
    -> lino_quest_start
+ Keep Walking.
    ->END

===talk_to_annika===
{met_annika:
    {lino_step==0 and lino_quest==true:
        ->annika_ask_qs
    -else:
        ->annika_chat_base
    }
-else:
    ->annika_meet_convo
}

===annika_meet_convo===
#speaker: Unknown
Ugh. This stupid city...
#speaker: Narrator
The young woman looks a bit frazzled and unfamiliar with the area.
* {lino_step==0 and lino_quest==true}[Excuse me, are you Annika?]
    #speaker: Unknown
    What?
    #speaker: Unknown
    Um, yes, that's me.
    ~met_annika=true
    #speaker: Annika
    Let me guess, Lino asked you to find me?
    #speaker: Narrator
    Annika sighs, then seems to loosen up a little.
    #speaker: Annika
    He's sweet, but I really need to do this on my own.
    ->annika_ask_qs
+ Hello, it's good to meet you.
    #speaker: Unknown
    Hmm, what?
    #speaker: Unknown
    Oh. I'm Annika.
    #speaker: Annika
    I'm a bit busy now but maybe we can talk later.
    ~met_annika=true
    ->END
    
*{dayCount==2} Do you need help with anything?
    #speaker: Annikia
    Ugh, I'm just trying to get around this city!
    #speaker: Annikia
    It's cold, the directions don't make sense, 
    #speaker: Annikia
    I don't even know why I came here in the first place.
    #speaker: Annikia
    The woman sighs.
    #speaker: Annikia
    Sorry, I know you were just trying to be helpful.
    ~met_annika=true
    ->annika_quest_offer
+ She seems busy, best to not bother her...
    ->END
===annika_ask_qs===
+ You don't need to do this all on your own. Is there anything that would be helpful?
    #speaker: Annika
    Thanks. Truthfully I can't think of anything. 
    #speaker: Annika
    I'm just overwhelmed, you know?
    #speaker: Annika
    I guess I left most of my clothing back home. And I didn't bring anything warm.
    #speaker: Annika
    Maybe my cousin has something I can borrow...
    #speaker: Annika
    And I still need to get everything ready for my job interview...
    #speaker: Annika
    In any case, thank you for asking. I'm starting to feel better now.
    ~annika+=1
    ~lino_step=2
    ->END
+ Wow, you are rude. I just wanted to help.
    #speaker: Annika
    Well, you're not helping.
    ~annika-=1
    ->END
->END
===annika_chat_base===
#speaker: Annika
Hey, {player_name}.
{lino_quest==true and lino_step==0:
    ->annika_ask_qs
}
{annika_quest==false:
    {annika_interviewed==false and lino_step>=4:
        {lino_gift=="pen":
            ->annika_chat_pen
        -else:
            {lino_gift=="shawl":
                ->annika_chat_shawl
            -else:
                {lino_gift=="chili":
                ->annika_chat_chili
                -else:
                {dayCount==2:
                    ->annika_quest_offer
                -else:
                    ->END
                    }
                }
            }
        }
    -else:
        {dayCount==2:
            ->annika_quest_offer
        -else:
            ->END
        }
    }
-else:
->annika_quest_progress
}
===annika_chat_shawl===
#speaker: Annika
Thank you for helping Lino pick out a gift for me.
#speaker: Annika
I feel so much warmer and at home in the city now.
#speaker: Annika
Nothing like a warm shawl to make you feel at home.
#speaker: Annika
~annika_interviewed=true
->END

===annika_chat_chili===
#speaker: Annika
Ugh, I feel sick.
#speaker: Annika
My dumb cousin got me a chili dog... I think it's gone bad...
~annika_interviewed=true
->END

===annika_chat_pen===
#speaker: Annika
Thank you for helping Lino pick out a gift for me.
#speaker: Annika
I didn't think I wanted anything, but...
#speaker: Annika
I really feel much more prepared for my interview now.
~annika_interviewed=true
->END

===annika_quest_offer===
/////////////////////////////////////////
//annika quests///////////////////////////
////////////////////////////////////////
#speaker: Annika
The thing is, I've finally gotten through my interview.
#speaker: Annika
Would you believe they've already offered me a job?
+ Wow, congratulations!
    ~annika+=smallChange
    ~friendly+=smallChange
    #speaker: Annika
    Thank you, I'm really excited about it.
    #speaker: Annika
    Though, I'm not sure if I'll take the job.
    ->annika_quest_start
+ Already? Sounds like a red flag...
    ~annika-=smallChange
    ~pragmatic+=smallChange
    #speaker: Annika
    Not necessarily.
    #speaker: Annika
    Though, I suppose you're right.
    #speaker: Annika
    Honestly, I'm not sure if I'll take the job.
    ->annika_quest_start
+ How do you feel about it?
    ~annika+=medChange
    ~thoughtful+=smallChange
    #speaker: Annika
    Honestly, I'm not sure how I feel about it.
    ->annika_quest_start

->END

===annika_quest_start===
    #speaker: Annika
    I'm not really familiar with the area.
    #speaker: Annika
    I don't know the public transit map.
    #speaker: Annika
    And I don't know if it's even close to my cousin's neighborhood.
    + Is there anything I can help with?
        ~thoughtful+=smallChange
        #speaker: Annika
        Maybe you could help me settle into the area.
        #speaker: Annika
        Can you see if you can find a bus plan?
        #speaker: Annika
        I think the traveling vendor should have one.
        #speaker Narrator
        Quest: Annika's New Job
        #speaker Narrator
        Next Objective: Get a Bus Pass from Stevie
        ~annika_quest=true
        ~annika_step=0
        ->END
    + Yeah, it sounds like that's something to think about. 
        #speaker: Annika
        I suppose so.
        ->END

===annika_quest_progress===
* {dayCount==2 and annika_step==0 and have_new_info_annika==false}[Can you remind me what you want me to find?]
    #speaker: Annika
    I need a bus plan from Stevie. He should be on this street somewhere.
    ->END
* {dayCount==2 and annika_step==0 and have_new_info_annika}[Here, I got the bus plan from Stevie.]
    #speaker: Narrator
    You give the bus plan to Annika.
    ~have_new_info_annika=false
    ~annika_step=1
    #speaker: Annika
    Oh, great. This is helpful.
    #speaker: Narrator
    Annika looks through the brochure for a moment.
    #speaker: Annika
    Good, good. This should work.
    #speaker: Annika
    Okay, the public transit seems like it should work okay.
    #speaker: Annika
    I still need to know which neighborhood Lino lives in, though.
    #speaker: Annika
    Would you mind asking him?
    #speaker Narrator
    Quest: Annika's New Job
    #speaker Narrator
    Next Objective: Ask Lino about his Neighborhood
    ~actionCount+=1
    ->action_check
* {dayCount==2 and annika_step==1 and have_new_info_annika==false}[Can you remind me what you want me to find?]
    #speaker: Annika
    Please ask Lino what neighborhood he lives in.
    ->END
* {dayCount==2 and annika_step==1 and have_new_info_annika}[I just talked to Lino; he lives in Reagent Village.]
    #speaker: Annika
    Ah, I see. Excellent, yes, it's right here on the brochure.
    #speaker: Annika
    Thank you, this has all been really useful information.
    #speaker: Narrator
    Annika sighs, her eyes cast downward a little disappointedly.
    #speaker: Annika
    Logistically, I can see myself living here.
    #speaker: Annika
    But I'm not sure if this is a spot I'd want to be.
    #speaker: Annika
    I wish I knew of something fun happening downtown...
    #speaker: Annika
    Somewhere I could meet new friends and engage in the culture of the city.
    #speaker: Annika
    Do you think you could find something like that? Maybe a club or something?
    ~helpful+=smallChange
    ~pragmatic+=smallChange
    ~annika+=smallChange
    ~have_new_info_annika=false
    ~annika_step=2
    ~ add_friend("annika")
    #speaker Narrator
    Quest: Annika's New Job
    #speaker Narrator
    Next Objective: Find an advertisement for an upcoming event.
    ~actionCount+=1
    ->action_check
* {dayCount==3 and annika_step==2 and have_new_info_annika==false}[Can you remind me what you want me to find?]
    #speaker: Annika
    I wish I knew of something fun happening downtown...
    #speaker: Annika
    Somewhere I could meet new friends and engage in the culture of the city.
    #speaker: Annika
    Do you think you could find something like that? Maybe a club or something?
    ->END
* {dayCount==3 and annika_step==2 and have_new_info_annika}[Any chance you'd be interested in joining a local band?]
    #speaker: Narrator
    You hand Annika the poster for the Four Gophers' performance.
    #speaker: Annika
    Hmm. The Four Gophers?
    #speaker: Annika
    Strange name for a band. But... that sounds like a really nice time. 
    #speaker: Annika
    Thank you, {player_name}, I think I'm starting to feel at home here.
    #speaker: Annika
    I'm really glad I was able to meet you!
    ~annika_quest=false
    ~annika_step = 3
    ~gophers_step=1
    ~annika+=bigChange
    ~lino+=bigChange
    ~friendly+=bigChange
    ~helpful+=bigChange
    ~have_new_info_annika=false
    #speaker Narrator
    Quest: Annika's New Job
    #speaker Narrator
    Quest Complete!
    #speaker Narrator
    Quest: The Four Gophers
    #speaker Narrator
    Next Objective: Tell Korra the Good News
    ~actionCount+=1
    ->action_check
* {annika_step==3}[Hi, Annika! Good to see you.]
    #speaker: Annika
    Likewise. I hope to see you at our concert on Friday!
    ->END
+ Nevermind, we'll talk later.
    #speaker: Annika
    Okay.
    ->END
    
===talk_to_korra===
{met_korra:
    ->korra_chat
-else:
    ->korra_meet_convo
}

===korra_chat===
#speaker: Korra
Howdy there, friend!
* {mural_step==0 and mural_quest==true}[Any chance you're interested in a mural?]
    #speaker: Korra
    A mural? I can't say I'm a fan of mushrooms.
    #speaker: Korra
    ...Oh! You said mural!
    #speaker: Korra
    Oooooooh, let me think. Yes, I think that would be perfect!
    #speaker: Korra
    We've got a stage back in the park where we like to perform, and it's been seeming pretty bare lately.
    #speaker: Korra
    Yeah, yeah, that would be a great way to liven things up.
    #speaker: Korra
    Please let August know we're in!
    ~mural_step=1
    ~mural_loc="park"
    #speaker: Narrator
    Quest: A Summer Mural
    #speaker: Narrator
    Next Objective: Tell August where they can paint their mural
    ->END
* {gophers_step==0 and four_gophers==true}[I'm still looking for a fourth gopher!]
    #speaker: Korra
    Thanks for doing that!
    #speaker: Korra
    Best of luck :-)
    ->END
* {gophers_step==1}[Good news! I found you a fourth gopher! Er- Band member!]
    #speaker: Korra
    Really? That's fantastic! 
    #speaker: Korra
    I'm looking forward to meeting them at practice.
    #speaker: Korra
    Thanks for your help, friend! :-)
    ~korra+=bigChange
    ~helpful+=bigChange
    ~friendly+=bigChange
    ~creative+=bigChange
    ~gophers_step=2
    ~four_gophers=false
    ~ add_friend("korra")
    #speaker Narrator
    Quest: The Four Gophers
    #speaker Narrator
    Next Objective: Quest Complete!
    ~actionCount+=1
    ->action_check
* {gophers_step==2}[Looiking forward to your concert this Friday!]
    #speaker: Korra
    You got it! It'll be a blast :-)
    ->END
+ Howdy! Talk to you later.
    #speaker: Korra
    You got it!
    ->END
    
===korra_meet_convo===
#speaker: Unknown
Howdy, there!
#speaker: Unknown
Any chance you're around this Friday evening?
+ Pardon? 
    #speaker: Unknown
    Oh, right, I guess I should introduce myself.
    #speaker: Unknown
    The name is Korra! :-D
    ~met_korra=true
    {dayCount==3:
        ->korra_introduce
    -else:
        ->korra_pre_day3
    }
+ Sure, sounds fun! What's going on?
    #speaker: Unknown
    Oh, wait, I guess I should introduce myself first.
    #speaker: Unknown
    The name is Korra! :-D
    ~adventurous+=smallChange
    ~friendly+=smallChange
    ~met_korra=true
    {dayCount==3:
        ->korra_introduce
    -else:
        ->korra_pre_day3
    }
    
===korra_pre_day3===
#speaker: Korra
I'm part of a local band.
#speaker: Korra
We've got a concert later this week, but we're still setting up.
#speaker: Korra
Hope to talk to you later :-)
->END

==korra_introduce===
#speaker: Korra
Anyways. I'm part of a 3-person alt-folk-punk-rock group. We're the Four Gophers!
+ But... there's three of you?
    ~pragmatic+=smallChange
    #speaker: Korra
    Well, that's our biggest problem.
    #speaker: Korra
    Ya see, we really thought we'd have another person by now. :-(
    #speaker: Korra 
    They don't have to do anything crazy, or anything. We just need someone on tambourine.
    #speaker: Korra 
    It really gives the whole thing another layer.
    #speaker: Korra 
    Do you think you could find us a fourth member?
    #speaker: Narrator
    Quest: The Four Gophers
    #speaker: Narrator
    Next Objective: Find a Fourth Member for the Four Gophers
    ~four_gophers=true
    ~have_new_info_annika=true
    ~gophers_step=0
    ->END
+ Cool, sounds like fun!
    ~adventurous+=smallChange
    #speaker: Korra
    It is!
    #speaker: Korra
    ...although, we do have an issue.
    #speaker: Korra
    Ya see, we really thought we'd have another person by now. :-(
    #speaker: Korra
    They don't have to do anything crazy, or anything. We just need someone on tambourine.
    #speaker: Korra
    It really gives the whole thing another layer.
    #speaker: Korra
    Do you think you could find us a fourth member?
    #speaker: Korra
    Here's a poster you can use.
    #speaker: Narrator
    Korra gives you a poster with four gophers holding musical instruments.
    #speaker: Narrator
    Quest: The Four Gophers
    #speaker: Narrator
    Next Objective: Find a Fourth Member for the Four Gophers
    ~have_new_info_annika=true
    ~four_gophers=true
    ~gophers_step=0
    ->END
    
===talk_to_august===
{met_august:
    ->august_chat
-else:
    ->august_meet_convo
}

===august_chat===
#speaker: August
Hello, there!
* {dayCount==3 and mural_quest==false and mural_step==0}[Is there anything I can help you with?]
    #speaker: August
    As a matter of fact, there is something I could use a hand with.
    ->august_quest_offer
* {mural_quest==true and mural_step==0}[Can you give me some ideas for where to ask about the mural?]
    #speaker: August
    Hmm. I think a stage, a school, or a somewhere in town would be a perfect location.
    #speaker: August
    Maybe ask around town. I suspect you have a friend who might be interested in a mural.
    ->END
* {mural_quest==true and mural_step==1}[Good news! I found a spot for your mural!]
    ->share_mural
* {mural_quest==false and mural_step==2}[How is the mural coming along?]
    #speaker: August
    Oh, it's coming along quite nicely. I'd rather not spoil it, but I look forward to showing you once it's done.
    ->END
+ Hello to you!
    ->END


===share_mural===
#speaker: August
The {mural_loc}, you say? That sounds... perfect!
#speaker: August
I'll get started on it right away.
#speaker: August
Thank you kindly for helping me with this. I'd rather not spoil my idea, but I know exactly what I'll be painting.
#speaker: August
I hope to catch up with you soon!
#speaker: Narrator
Quest: A Summer Mural
#speaker: Narrator
Quest Complete!
~ add_friend("august")
~mural_quest=false
~mural_step=2
~actionCount+=1
->action_check

===august_quest_offer===
#speaker: August
You see, I'm planning my next artwork. It will be a mural illustrating the instrumental connection between community and nature.
#speaker: August
However, I haven't the foggiest idea of where to paint it.
#speaker: August
I have some ideas, but I've been here so long that I'm used to everything.
#speaker: August
You have a fresh pair of eyes. Maybe you'd be willing to ask around for me?
+ Of course! I'd be happy to help.
    #speaker: August
    Excellent!
    #speaker: August
    I'm sure there are plenty of spots around town.
    #speaker: August
    Maybe ask some of the neighbors? I think a stage, a school, or a somewhere in town would be a perfect location.
    #speaker: Narrator
    Quest: A Summer Mural
    #speaker: Narrator
    Next Objective: Find a new Art Site for August
    ~mural_quest=true
    ->END
+ That's not really my thing.
    #speaker: August
    I see. Well, I suppose I'll make do.
    ->END

===august_meet_convo===
#speaker: Unknown
Hmm.
#speaker: Unknown
Ah - no, that's not it.
#speaker: Unknown
But perhaps...? No, not that, either.
*{dayCount==3}[Hello there, do you need something?]
    #speaker: Unknown
    Hmm, what's that?
    #speaker: Unknown
    Ah! You're new here, aren't you?
    #speaker: Unknown
    ...yes, I suppose I do need help. But we should be introduced first, shouldn't we?
    #speaker: Unknown
    My name is August. I'm the local painter here.
    #speaker: August
    As for what Im looking for a hand with...
    ->august_quest_offer
+ You look like a friendly face. What's your name?
    #speaker: Unknown
    Well aren't you polite.
    #speaker: Unknown
     I'm August, the local painter.
    #speaker: August
    I'm fairly well known around these parts. And, by extension, I know most of the folks here. You're new to the area, aren't you?
    #speaker: August
     Well, no matter, it is a pleasure to meet you. You're going to love it here.
    ~met_august=true
    ->END
+ You're talking to yourself awfully loudly.
    #speaker: Unknown
    Ah, my apologies. I let my mind get away from me.
    #speaker: Unknown
    Then again, that tends to happen. You're new around here, so I imagine you're not used to it yet. I'm August, the local painter, and this sort of thing will happen a lot.
    #speaker: Unknown
    So, if you'll excuse me.
    ~met_august=true
    ->END

    
=== stevie_meet_convo ===
{met_stevie:
-> stevie_quest_convo
-else:
->stevie_first_convo
}
=== stevie_first_convo ===
#speaker: Unknown
Hey! You, there!
+ Who? Me?
    #speaker: Unknown
    Yes, you! In the low-poly bearings!
    #speaker: Player
    Low-poly bearings...?
    #speaker: Unknown
    The name is Stevie. Connoisseur of doo-dads, knick-knacks, and whatchamacallits.
    #speaker: Stevie
    Pleasure to meet you!
    ~met_stevie=true
    -> stevie_quest_convo
    
+ Keep Walking
    -> END
    
===stevie_score_check===
{stevieSales >= 2 and stevie_already_friend==false:
    ->stevie_friendship
-else:
    ->END
}

===stevie_friendship===
#speaker: Stevie
Just wanted to say thanks for supporting my business these past few days.
#speaker: Stevie
Any time you need a business partner, you just let me know.
~ add_friend("stevie")
~stevie_already_friend = true
->END

=== stevie_quest_convo ===
#speaker: Stevie
Hello, there! What can I do ya for?
* {emily_charlie_step==1}[Any chance you have a communication device of some sort?]
    #speaker: Stevie
    You betcha! Here, take a look at what I just got in.
    #speaker: Narrator
    Stevie pulls out a pair of strange gray boxes decorated with an antenna and a variety of stickers.
    #speaker: Stevie
    These should work well for your purposes. 
    #speaker: Stevie
    They've got a radius of up to the whole game map.
    ->walkies
* {lino_step==2}[I'm looking for a gift. Do you have anything like that?]
    #speaker: Stevie
    Oh, do I ever - is it for a special someone? 
    #speaker: Stevie
    Well, whoever it's for, I've got you covered.
    #speaker: Narrator
    Stevie pulls out a trunk from nowhere and opens it up.
    #speaker: Stevie
    You've got three options, my good friend:
    #speaker: Stevie
    ONE! A baby blue cashmere shawl, as soft as a baby goat's behind.
    #speaker: Stevie
    TWO! A fountain pen with a polished bronze nib. Comes with jet black ink.
    #speaker: Stevie
    And THREE! A Philadelphia Chili Cheese Dog - still warm from when I cooked it this morning.
    #speaker: Stevie
    They all happen to be the same price of - 
    #speaker: Narrator
    Stevie peeks at the money in your hand to gauge what you're holding.
    #speaker: Stevie
    Thirty tickets! What do you say?
    ->gift
* {annika_step==0 and annika_quest}[Do you have a bus plan, by any chance?]
    #speaker: Stevie
    Aha, yes! You are in luck!
    #speaker: Stevie
    I do in fact have a bus plan right here.
    #speaker: Narrator
    Stevie pulls a dark blue brochure from his vest pocket and opens it up to show you...
    #speaker: Narrator
    A twelve-panel brochure?!
    #speaker: Stevie
    As you can see it is equipped with all of the transport information one could ever need!
    #speaker: Stevie
    Though, as you can imagine, it costs a pretty penny...
    #speaker: Stevie
    What do you say to...
    #speaker: Stevie
    One thousand tickets?
    ->bus_pass
+ Nothing right now, thanks.
    #speaker: Stevie
    Well, come back if you need anything!
    ->stevie_score_check

===walkies===
+ ...game map?
    #speaker: Stevie
    Eh, it's an inside joke.
    #speaker: Stevie
    Now, normally these puppies would go for twenty tickets or so.
    #speaker: Stevie
    But I'm in a good mood. So I'll give them to you for free. As a promotional item.
    #speaker: Narrator
    Stevie hands you the walkie-talkies.
    ~has_walkies=true
    ~emily_charlie_step=2
    ~stevie+=3
    ~stevieSales+=1
    #speaker: Stevie
    Just remember to come back to me if you need anything else!
    #speaker: Stevie
    You won't get a better deal anywhere else.
    #speaker: Narrator
    Quest: Emily and Charlie
    #speaker: Narrator
    Next Objective: Give Walkie-Talkies to Emily
    ->stevie_score_check

+ ha! That's perfect!
    ~funny+=medChange
    #speaker: Stevie
    I knew you'd get it!
    #speaker: Stevie
    Now, normally these puppies would go for twenty tickets or so.
    #speaker: Stevie
    But I'm in a good mood. So I'll give them to you for free. As a promotional item.
    #speaker: Narrator
    Stevie hands you the walkie-talkies.
    ~has_walkies=true
    ~emily_charlie_step=2
    ~stevie+=3
    ~stevieSales+=1
    #speaker: Stevie
    Just remember to come back to me if you need anything else!
    #speaker: Stevie
    You won't get a better deal anywhere else.
    #speaker: Narrator
    Quest: Emily and Charlie
    #speaker: Narrator
    Next Objective: Give Walkie-Talkies to Emily
    ->stevie_score_check

->END
===gift===
+ The Shawl.
    #speaker: Stevie
    Ah, a warm gift for a warm soul.
    #speaker: Stevie
    I almost regret parting with such a fine item.
    #speaker: Stevie
    But I am sure it will find a good home with you and your friend.
    #speaker: Narrator
    You hand Stevie the money and accept the shawl, folding it nicely as you do so.
    #speaker: Stevie
    Best wishes to you and your friend.
    #speaker: Stevie
    Remember to come back to me if you need anything else!
    #speaker: Narrator
    Quest: A Gift for Annika
    #speaker: Narrator
    Next Objective: Give Lino the Gift
    ~lino_gift="shawl"
    ~lino_step=3
    ~friendly+=smallChange
    ~thoughtful+=smallChange
    ~stevieSales+=1
    ~stevie+=3
    ->stevie_score_check
    
+ The Fountain Pen.
    #speaker: Stevie
    Aha, an intelligential gift for an intelligential folk!
    #speaker: Stevie
    I will warn you to be careful with this...
    #speaker: Stevie
    Lest you or your friend get carried away with poetry and become the next big name.
    #speaker: Narrator
    You hand Stevie the money and accept the fountain pen.
    #speaker: Stevie
    There you are! Best wishes with the gift.
    #speaker: Narrator
    Quest: A Gift for Annika
    #speaker: Narrator
    Next Objective: Give Lino the Gift
    ~lino_step=3
    ~lino_gift="pen"
    ~pragmatic+=smallChange
    ~friendly+=smallChange
    ~stevie+=3
    ~stevieSales+=1
    ->stevie_score_check
    
+ The Chili Dog!
    #speaker: Stevie
    Excellent choice, my good lady!
    #speaker: Stevie
    I can attest these are the most excellent of chili dogs.
    #speaker: Narrator
    You hand Stevie the money for the chili dog.
    #speaker: Stevie
    Let me wrap it up all nice for you.
    #speaker: Narrator
    Stevie crinkles up the aluminum foil, then wraps the whole thing with a gaudy pink bow.
    #speaker: Stevie
    There you are! Best wishes with the gift.
    #speaker: Narrator
    Quest: A Gift for Annika
    #speaker: Narrator
    Next Objective: Give Lino the Gift
    ~lino_step=3
    ~lino_gift="chili"
    ~funny+=medChange
    ~friendly+=smallChange
    ~stevie +=10
        ~stevieSales+=1
    ->stevie_score_check
    
+ I'm not sure. I'll think about it and get back to you later.
    #speaker: Stevie
    Understood, understood.
    #speaker: Stevie
    But don't take too long; these gifts go fast!
    ->stevie_score_check
===bus_pass===
+ One thousand?! That's egregious!
    ->bus_pass_continue
+ I don't have that much...
    ->bus_pass_continue
===bus_pass_continue===
    #speaker: Stevie
    Now, don't panic.
    #speaker: Stevie
    You don't have to pay that sum all at once!
    #speaker: Stevie
    I'll simply open a tab for you...
    #speaker: Stevie
    ...at a steep interest rate, of course.
    #speaker: Stevie
    What do you say to 72% APR?
    + ...Fine. This currency seems made up, anyways.
        #speaker: Stevie
        A wise decision! Here you are, good madame. 
        #speaker: Narrator
        Stevie hands you the bus guide.
        #speaker: Stevie
        Best wishes!
        ~have_new_info_annika=true
        ~funny+=1
        ~stevieSales+=1
        ->stevie_score_check
    + Umm, I suppose that's alright? (Maybe Annika will pay me back...)
        #speaker: Stevie
        A wise decision! Here you are, good madame. 
        #speaker: Narrator
        Stevie hands you the bus guide.
        #speaker: Stevie
        Best wishes!
        ~have_new_info_annika=true
        ~pragmatic+=1
        ~stevieSales+=1
        ->stevie_score_check
    + That's too much. Sorry, Stevie, maybe next time.
        #speaker: Stevie
        Alright, well, come back if you change your  mind!
        #speaker: Stevie
        I can't guarantee I'll still have it on hand the next time you need it.
        ->stevie_score_check
    
===talk_to_jojo===
{met_jojo:
    ->jojo_chat
-else:
    ->jojo_meet_convo
}
    
===jojo_meet_convo===
#speaker: Unknown
Hey! You're new!
+ Yes, I'm new to town! It's nice to meet you.
    #speaker: Unknown
    Oh, okay. I'm Jojo.
    ~met_jojo=true
    ->jojo_chat
+ Umm. Shouldn't you be in school? Or something?
    #speaker: Unknown
    It's summer break. Don't you know that?
    #speaker: Unknown
    Adults these days don't know anything...
    #speaker: Unknown
    I'm Jojo. I'll help you get used to this town in no time.
    ~met_jojo=true
    ->jojo_chat
+ And you're weird.
    #speaker: Unknown
    That's mean. You're not supposed to say stuff like that.
    ->END
    
===jojo_chat===
#speaker: Jojo
Hey, lady.
{paint and gum and handshake:
~ add_friend("jojo")
}
{dayCount==1 and handshake==false:
    ->jojo_day1
-else:
    {dayCount==2 and paint==false:
        ->jojo_day2
    -else:
        {dayCount==3 and gum==false :
            ->jojo_day3
        -else:
            ->jojo_base
        }
    }
}

===jojo_base===
+ Hey, Jojo!
    #speaker: Jojo
    Hi! I'm busy but I'll see you tomorrow.
    ->END
+ I've got to go, see you later.
    #speaker: Jojo
    Okay, see ya!
    ->END

===jojo_day1===
#speaker: Jojo
Oh hey, since we're neighbors now...
#speaker: Jojo
Can I show you my secret handshake?
->jojo_handshake

===jojo_handshake===
#speaker: Narraator
Jojo doesn't wait for your response, instead holding up his hands. He bounces impatiently, waiting for you to do the same.
#speaker: Jojo
Okay. Up High...
#speaker: Narrator
You've definitely seen this game before. You decide to play along.
#speaker: Narrator
You give Jojo a high-five up high.
#speaker: Jojo
Now down low...
#speaker: Narrator
You give Jojo a high-five down low.
#speaker: Jojo
Up in space...
+ Play along and high-five Jojo again
    #speaker: Narrator
    You high-five Jojo again.
    #speaker: Jojo
    IN YOUR FACE!
    #speaker: Narrator
    Jojo gets on his tippy-toes to high-five your face. He misses by about a foot.
    #speaker: Jojo
    Ha! You totally fell for it!
    #speaker: Jojo
    That was really fun. Thank you for playing with me.
    ~handshake=true
    ~jojo +=1
    ->END
+ In your face!
    #speaker: Narrator
    Beating Jojo to the punchline, you bop him on the forehead while his arms are way up in the air.
    #speaker: Narrator
    You didn't bop him that hard, but he starts to tear up.
    #speaker: Jojo
    Hey, that's not... that's not fair.
    #speaker: Jojo
    You skipped ahead. You cheated.
    #sspeaker: Narrator
    Jojo turns away from you in a pout.
    ->END
+ ...Up in space?
    #speaker: Jojo
    Ugh. It's part of the handshake. We have to restart it now.
    ->jojo_handshake


===jojo_day2===
#speaker: Jojo
Hi, {player_name}! Can you do me a big favor?
#speaker: Jojo
I'm doing an art project but I need something from the store.
#speaker: Jojo
Can you buy me striped paint?
+ Horizontal or vertical stripes?
    #speaker: Jojo
    Gotcha!
    #speaker: Jojo
    Striped paint isn't real.
    #speaker: Jojo
    You'd need to get two different colors and paint the stripes yourself, silly.
    #speaker: Jojo
    I was just messing with you. You're so gullible!
    ~jojo+=1
    ~paint=true
    ->END
+ I think you'd need to get two different colors and make the stripes yourself.
    #speaker: Jojo
    Ugh, you're no fun. I guess you've heard that one before...
    ->END

===jojo_day3===
#speaker: Jojo
Hi {player_name}, thanks for hanging out with me so much.
#speaker: Jojo
As a thank you, can I give you a piece of gum?
#speaker: Narrator
Jojo holds out a pack of gum. It's no brand of gum you've ever seen before, and one piece is clearly sticking out.
#speaker: Narrator
You think you know what's happening here.
+ Thanks, Jojo! I'll take a piece.
    #speaker: Narrator
    You take a piece, bracing yourself for the electric shock.
    #speaker: Narrator
    ZZZAAAAAAAAP!
    #speaker: Jojo
    Haha, you totally fell for it!
    #speaker: Jojo
    Uh, sorry to trick you. I have some real gum in my pocket.
    #speaker: Narrator
    Jojo apologetically hands you a piece of gum. It's real this time.
    #speaker: Jojo
    Thanks for playing with me. My friends don't let me use that one anymore.
    ~jojo +=1
    ~gum=true
    {paint and gum and handshake:
    ~ add_friend("jojo")
    }
    ->END
+ Thanks for the offer, but I'm alright for now.
    #speaker: Jojo
    Oh! Well, maybe you can take a piece for later?
    #speaker: Jojo
    Or, I guess we can just talk later...
    ->END
+ You're just trying to shock me! I'm not falling for it.
    #speaker: Jojo
    Oh. Uh, no! That's silly.
    #speaker: Jojo
    This is real gum, honest!
    #speaker: Jojo
    Um, maybe later, I guess...
    ->END
    
    ===function add_friend(friend_name)===
    ~return
    
===friend_wrapup===
->END
