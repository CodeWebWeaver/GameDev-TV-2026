//For reference:
//When using in unity, you should be able to just call the "talk_to_name" knots - those will branch out to relevant knots based on the global flags.
VAR player_name = "Player"
VAR player_friends_count = 0

VAR dayCount = 0

//player attributes
VAR name="placeholder"
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

//dog quest
//step 0: talk to beverly for a description of where the dog might be
//step 1: find the dog and speak in dog language to get him to go home
//step 2: talk to beverly and confirm that coco made it home
//step 3: give charlie the good news
VAR dog_quest = false
VAR dog_step = 0 //0-3
VAR from_charlie=false


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

//friendship scores
VAR emily = 0
VAR charlie = 0
VAR beverly = 0
VAR stevie = 0
VAR lino = 0
VAR coco = 0
VAR annika = 0
VAR korra = 0

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


/////////////////////////////////////////
//dog quests////////////////////////////
/////////////////////////////////////////
=== talk_to_coco===
{dog_step==0:
    ->coco_meet_convo
-else:
    {dog_step==1:
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
        ->charlie_day_one
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
    I'm grounded for a whole week, you know.
    Come back once you have good news.
    ~charlie-=1
    ->END
+ Yes, and I have good news.
    #speaker: Charlie
    You found Coco?
    That's great news!
    Whew, now that that's taken care of, I can figure out how to get out of my grounding situation...
    #speaker: Narrator
    Quest: The Lost Dog
    Quest Complete!
    ~charlie+=5
    ~dog_step=4
    ~dog_quest=false
    ~helpful+=medChange
    ~pragmatic+=medChange
    ~friendly+=medChange
    ->END
    
+ I found Coco!
    #speaker: Charlie
    Yes!!! I knew we could do it!
    I bet Beverly is glad.
    Ah - don't let my mom know I called her by her first name!
    #speaker: Narrator
    Quest: The Lost Dog
    Quest Complete!
    ~charlie+=5
    ~dog_step=4
    ~dog_quest=false
    ~helpful+=medChange
    ~adventurous+=medChange
    ~friendly+=medChange
    ->END
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
            ->beverly_quest_start
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
    Oh, I'm sorry, dear. Coco is my sweet dog.
    She hasn't come back in a while.
    ->beverly_quest_offer
+ Oh no, is everything alright?
    #speaker: Beverly
    Truthfully, not at all.
    My sweet dog Coco wandered off yesterday afternoon and hasn't returned.
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
* {dog_step==0} [Can you give me a description of your lost dog?]
    #speaker: Beverly
    Hmm? Coco?
    Does that mean you'll help me look? Thank you, dear.
    He is fluffy, gray, and about knee-height.
    He prefers to answer to "Wuf-Wuf" rather than "Woof-Woof". I think he still has an accent from the southern region.
    He doesn't like to stray far; he should still be in the neighborhood somewhere.
    #speaker: Narrator
    Quest: The Lost Dog
    Next Objective: Find Coco
    ~dog_step=1
    ~pragmatic+=smallChange
    ~helpful+=smallChange
    ~beverly+=1
    ->END
* {dog_step==0 and from_charlie} [Charlie told me your dog is lost, I am so sorry! How can I help?]
    #speaker: Beverly
    Thank you, dear. I'm really beside myself.
    It was so helpful of Charlie to help me look. Coco adores him.
    If you wouldn't mind helping me look for Coco...
    He is fluffy, gray, and about knee-height.
    He prefers to answer to "Wuf-Wuf" rather than "Woof-Woof". I think he still has an accent from the southern region.
    Thank you for your help, dear.
    #speaker: Narrator
    Quest: The Lost Dog
    Next Objective: Find Coco
    ~dog_step=1
    ~helpful+=smallChange
    ~thoughtful+=smallChange
    ~beverly+=1
    ->END
* {dog_step<=2 and met_coco}[I think I saw Coco down the street!]
    #speaker: Beverly
    What? That's great news!
    It's too far for me to walk, though.
    Could you please tell him to come home?
    He prefers to answer to "Wuf-Wuf" rather than "Woof-Woof". I think he still has an accent from the southern region.
    ->END
* {dog_step==0} [I'm busy now, we'll talk later.]
    ->END
* {dog_step==1} [Can you remind me what Coco looks like?]
    #speaker: Beverly
    Of course, dear.
    He is fluffy, gray, and about knee-height.
    He prefers to answer to "Wuf-Wuf" rather than "Woof-Woof". I think he still has an accent from the southern region.
    Thank you for your help, dear.
    ->END
* {dog_step==1} [I think I have what I need, I'll talk to you later.]
    ->END
* {dog_step==2}[Hi, Beverly! I think I found Coco. Did he make it home okay?]
    #speaker: Beverly
    Yes, he ran home just a minute ago!
    I think he's napping now - I gave him a whole tray of biscuits as a reward.
    #speaker: Narrator
    That sounds like too many biscuits for a dog...
    Still, I'm glad he made it home.
    #speaker: Beverly
    I can't tell you how grateful I am. If you ever need a favor, you know where to find me.
    Oh, would you tell Charlie that Coco came home?
    I'm sure he'd be relieved to hear it!
    #speaker: Narrator
    Quest: The Lost Dog
    Next Objective: Check in with Charlie
    ~dog_step=3
    ~helpful+=medChange
    ~adventurous+=smallChange
    ~beverly+=5
    ~coco+=5
    ->END
    
===beverly_chat===
#speaker: Beverly
Ah, ~name, it's lovely to see you, dear!
Do you need help with anything?
+ No, thank you!
    #speaker: Beverly
    Well, you know where to find me if you need anything, dear!
    ->END
===coco_meet_convo===
#speaker: Unknown
Wuf, Wuf!
#speaker: Narrator
A strange gray dog looks at you, wagging his tail.
You think he's trying to tell you something.
~met_coco=true
->END
===coco_come_home===
#speaker: Coco
Wuf-Wuf!
#speaker: Narrator
Coco looks like he wants to talk with you.
Maybe you can tell him to come home?
+ Woof! Woof!
    #speaker: Coco
    Wuf. Grrrr.
    #speaker: Narrator
    Coco's ears flatten and he sits back, annoyed.
    Whatever you said, it wasn't very polite!
    ->END
+ Bow-wow!
    #speaker: Coco
    Wuf. 
    #speaker: Narrator
    Coco shakes his head side-to-side.
    He doesn't seem to understand you.
    ->END
+ Wuf-Wuf!
    #speaker: Coco
    Wuf? 
    #speaker: Narrator
    Coco tips his head to the side.
    #speaker: Coco
    Wuf! Wuf-Wuf!
    #speaker: Narrator
    It seems Coco can understand you!
    Coco runs back home.
    You should probably see if he made it back okay.
    Quest: The Lost Dog
    Next Objective: Check in with Beverly
    ~friendly+=smallChange
    ~adventurous+=smallChange
    ~coco+=5
    ~dog_step=2
    ->END
+ Scratch Coco behind the ears.
    #speaker: Narrator
    Coco leans into your hand and gives a big shake.
    ->END
+ Try to pull Coco back home.
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
    I'm a little busy at the moment.
    My name is Beverly, if you need anything.
    ~met_beverly=true
    -> talk_to_beverly
*{dog_quest} [Excuse me, are you Beverly?]
    #speaker: Unknown
    Hmm?
    Oh, yes. You were looking for me?
    ~met_beverly=true
    -> talk_to_beverly
+ Keep Walking.
    ->END
=== charlie_day_one ===

{ met_charlie:

-> charlie_first_convo

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
=== charlie_first_convo ===

+ What are you doing up there?

->lost_dog_overview

* Do I know you?
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
But it’s not my fault!
My neighbor’s dog got loose and I was just trying to track it down…
I would have stayed out later than that. But mom was already so mad…
She wouldn’t even listen to my reason for staying out!

+  That’s not fair at all. Maybe I can talk to your mom and explain the situation to her.
    ~ ambitious += 1
    ~ easygoing += 1
    ~ charlie += 1
    ~ emily+= 1
    ~ emily_charlie_quest = true
    #speaker: Charlie
    You would do that for me?
    Hmm, maybe she *would* listen to another adult…
    Well, thanks! You should be able to find her on the street somewhere. 
    I think her name is Beverly? I’m not supposed to call her that, though…
    #speaker: Narrator
    Quest started: Unite Beverly and Charlie
    Next Objective: Find Beverly
    
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
    My neighbor who lost the dog lives down the street. My mom calls her Beverly.
    She should be able to tell you more.
    
    #speaker: Narrator
    Quest started: The Lost Dog
    Next Objective Find Emily
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
Hey! What are you up to?
+ Not much, still grounded?
    #speaker: Charlie
    Ugh. Yes, another 6 days, I think.
    Wish mom would just let me out already...
    ->END
+ Errands upon errands...
    Oof. I'd take grounding over that any day...
    ->END
    
    
    
===charlie_post_walkies===
#speaker: Charlie
Hey! I'm not grounded anymore!
+ Excellent!
    #speaker: Charlie
    Thanks for talking to my mom.
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
        ->emily_day_one
    }
-else:
->emily_meet_convo
}
===talk_to_shopkeeper===
{met_stevie:
->END
-else:
->stevie_meet_convo
}
=== emily_day_one ===
{emily_charlie_quest:
    {met_emily:
        {emily_charlie_step==0:
            ->emily_charlie_questintro
        -else:
            ->emily_charlie_step1
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
Kids these days...
+ Are you Emily?
    ->emily_charlie_overview
+ Keep Walking.
    ->END
    
=== emily_charlie_overview ===
#speaker: Unknown
Hmm?
Oh! Yes, I'm Emily. And you are...?
#speaker: Player
I'm ~name.
~met_emily = true
->emily_charlie_questintro

===emily_charlie_questintro===
+ Your son Charlie asked me to speak with you.
    #speaker: Emily
    Ugh, I should have known he'd try to bother a stranger.
    Not sure why I ever gave him the room with a window...
    #speaker: Narrator
    Emily sighs.
    -> emily_charlie_reaction
+ Did you hear that Beverly's dog is missing?
    ~ambitious+=smallChange
    ~friendly+=smallChange
    #speaker: Emily
    Oh no, how awful!
    Though, now that you mention it, Charlie did say something about a lost dog.
    Maybe he was helping Beverly look for it?
    If that's the case, then I feel awful for grounding him...
    As much as I'd like him to help Beverly, though, I need to be able to check in on him.
    The world is a scary place, after all.
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
    I feel awful for grounding him, then.
    But...
    I still need to know where he's at.
    The world is a scary place after all...
    ->emily_charlie_step1
+ Thank you, you should really keep an eye on him, though.
    ~friendly-=smallChange
    ~thoughtful-=smallChange
    #speaker: Emily
    You're right.
    I try, I really do.
    But since I'm working so often, it's hard to know what he's doing.
    And I can never find him when I need him.
    I'd like for him to be able to explore,
    But I need to be able to check in on him.
    ->emily_charlie_step1
    
=== emily_charlie_step1 ===
#speaker: Emily
You're a young person. You probably have a better idea of these things.
Do you think you can find a way for me to stay in touch with my son?
That way he can help Beverly look for her dog, and I can contact him when I need to check-in.
~emily_charlie_step=1
#speaker: Narrator
Quest: Emily and Charlie
Next Objective: Find Communication Device
->END



//Stevie convos
===emily_walkies===
#speaker: Emily
You came back; that was fast.
Any news to share?
* {has_walkies} [Give Emily the Walkie-Talkies]
    #speaker: Emily
    Oh my goodness!
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
    Charlie and I might even search the neighborhood together with these.
    Thank you, {name}, I can't tell you how much I appreciate your help!
    ~emily_charlie_step=3
    ~emily_charlie_quest=false
    ~has_walkies=false
    ~emily+=5
    ~charlie+=5
    ~pragmatic+=medChange
    ~helpful+=bigChange
    ->END
    
+ Not yet, I'll come back later.
    #speaker: Emily
    Okay, please don't take too long.
    ->END
===emily_post_walkies===
#speaker: Emily
Hello, {name}!
Thanks again for your help earlier; I'm having so much fun with these walkie-talkies.
I think Charlie might be getting a little sick of them!
#speaker: Charlie
...roger, mom. don't hold down the button when you're not transmitting...
+ It sounds like he's enjoying them, too!
    #speaker: Charlie
    ...roger!...
    ...these are really cool...
    #speaker: Narrator
    Emily laughs and releases the button
    #speaker: Emily
    I can't remember the last time the two of us have played together like this.
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
    ->lino_quest_start
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
Hello, {name}!
Did you find something?
* {lino_gift=="pen"}[Give Lino the fountain pen]
    #speaker: Lino
    Stunning! Annika is going to love this.
    She has a job interview coming up. This will be just the confidence boost she needs.
    Thank you for finding this. I really do appreciate it.
    ~lino+=5
    ~helpful+=bigChange
    ~pragmatic+=bigChange
    ~lino_step=4
    ~lino_quest=false
    #speaker: Narrator
    Quest: A Gift for Annika
    Quest Complete!
    ->END
* {lino_gift=="shawl"}[Give Lino the shawl]
    #speaker: Lino
    Oh, incredible.
    #speaker: Narrator
    Lino accepts the soft bundle of fabric.
    #speaker: Lino
    It's so soft and warm. And it looks just like something she used to wear at home.
    Thank you, truly. She is going to love this.
    ~lino+=5
    ~helpful+=bigChange
    ~thoughtful+=bigChange
    ~lino_step=4
    ~lino_quest=false
    #speaker: Narrator
    Quest: A Gift for Annika
    Quest Complete!
    ->END
* {lino_gift=="chili"}[Give Lino the chili dog]
    #speaker: Lino
    Eh... what is that?
    #speaker: Narrator
    Lino accepts the foil-wrapped chili dog and sniffs it.
    Lino retches
    #speaker: Lino
    Ugh, where did you find this?
    I don't have any money left for another gift... I suppose this will have to do...
    ~lino-=5
    ~thoughtful-=smallChange
    ~funny+=smallChange
    #speaker: Narrator
    Quest: A Gift for Annika
    Quest Complete!
    ->END
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
How are you? What can I do for you?
* {annika_step==1}[Lino, can you remind me which neighborhood you live in?]
    #speaker: Lino
    Ah, yes, of course!
    I am over on the east side of town.
    Formally, it's called Reagent Village...
    But we just call it the Orange Grove!
    ->END
+ I'm alright at the moment, but thank you!
    #speaker: Lino
    Alright, we will talk again soon!
    ->END

===lino_chat_negative===
#speaker: Lino
I don't really want to talk to you right now...
I think my sister is sick now...
->END

===lino_quest_start===
+ You look like you need help with something.
    #speaker: Lino
    That's okay, but...
    Hmm. Actually...
    #speaker: Narrator
    Lino pauses and scratches his chin.
    #speaker: Lino
    Maybe you could help me after all.
    I'm trying to find a gift for my cousin.
    You see, she just moved to the area.
    I'd love to get her a gift to welcome her to the city...
    #speaker: Narrator
    Lino sighs.
    #speaker: Lino
    But I'm terrible with gifts.
    You, though - you seem to be about her age.
    Could you pick out a gift for me?
    #speaker: Narrator
    Before you have a chance to respond, Lino presses the money into your hand.
    #speaker: Lino
    Her name is Annika. She is a strong, smart girl and she deserves the best!
    Please bring me whatever you find for her.
    ~lino_quest=true
    ~lino_step=0
    #speaker: Narrator
    Quest: A Gift for Annika
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
Not that one, either.
+ Hello there!
    #speaker: Unknown
    Oh! Hello!
    My name is Lino. 
    #speaker: Lino
    ~met_lino=true
    Can I help you with anything?
    -> lino_quest_start
+ Keep Walking.
    ->END

===talk_to_annika===
{met_annika:
    {lino_step==1:
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
* {lino_step==1}[Excuse me, are you Annika?]
    #speaker: Unknown
    What?
    Um, yes, that's me.
    ~met_annika=true
    #speaker: Annika
    Let me guess, Lino asked you to find me?
    #speaker: Narrator
    Annika sighs, then seems to loosen up a little.
    He's sweet, but I really need to do this on my own.
    ->annika_ask_qs
    
+ Do you need help with anything?
    #speaker: Unknown
    Ugh, I'm just trying to get around this city!
    It's cold, the directions don't make sense, 
    I don't even know why I came here in the first place.
    #speaker: Narrator
    The woman sighs.
    ???: Sorry, I know you were just trying to be helpful.
    ???: My name is Annika. 
    ~met_annika=true
    ->annika_ask_qs
+ She seems busy, best to not bother her...
    ->END
===annika_ask_qs===
+ You don't need to do this all on your own. Is there anything that would be helpful?
    #speaker: Annika
    Thanks. Truthfully I can't think of anything. 
    I'm just overwhelmed, you know?
    I guess I left most of my clothing back home. And I didn't bring anything warm.
    Maybe my cousin has something I can borrow...
    And I still need to get everything ready for my job interview...
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
Hey, {name}.
{annika_quest==false:
    {lino_gift=="pen":
        ->annika_chat_pen
    -else:
        {lino_gift=="shawl":
            ->annika_chat_shawl
        -else:
            {lino_gift=="chili":
            ->annika_chat_chili
            -else:
            ->END
            }
        }
    }
-else:
->annika_quest_progress
}
===annika_chat_shawl===
#speaker: Annika
Thank you for helping Lino pick out a gift for me.
I feel so much warmer and at home in the city now.
Nothing like a warm shawl to make you feel at home.
~annika_interviewed=true
->END
===annika_chat_chili===
#speaker: Annika
Ugh, I feel sick.
My dumb cousin got me a chili dog... I think it's gone bad...
->END
===annika_chat_pen===
#speaker: Annika
Thank you for helping Lino pick out a gift for me.
I didn't think I wanted anything, but...
I really feel much more prepared for my interview now.
~annika_interviewed=true
->END

===annika_quest_offer===
/////////////////////////////////////////
//annika quests///////////////////////////
////////////////////////////////////////
#speaker: Annika
Whew. I've finally gotten through my interview.
Would you believe they've already offered me a job?
+ Wow, congratulations!
    ~annika+=smallChange
    ~friendly+=smallChange
    #speaker: Annika
    Thank you, I'm really excited about it.
    Though, I'm not sure if I'll take the job.
    ->annika_quest_start
+ Already? Sounds like a red flag...
    ~annika-=smallChange
    ~pragmatic+=smallChange
    #speaker: Annika
    Not necessarily.
    Though, I suppose you're right.
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
    I don't know the public transit map.
    And I don't know if it's even close to my cousin's neighborhood.
    + Is there anything I can help with?
        ~thoughtful+=smallChange
        Maybe you could help me settle into the area.
        Can you see if you can find a bus plan?
        I think the traveling vendor should have one.
        #speaker Narrator
        Quest: Annika's New Job
        Next Objective: Get a Bus Pass from Stevie
        ~annika_quest=true
        ~annika_step=0
        ->END
    + Yeah, it sounds like that's something to think about. 
        #speaker: Annika
        I suppose so.
        ->END

===annika_quest_progress===
* {annika_step==0 and have_new_info_annika==false}[Can you remind me what you want me to find?]
    #speaker: Annika
    I need a bus plan from Stevie. He should be on this street somewhere.
    ->END
* {annika_step==0 and have_new_info_annika}[Here, I got the bus plan from Stevie.]
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
    Okay, the public transit seems like it should work okay.
    I still need to know which neighborhood Lino lives in, though.
    Would you mind asking him?
    #speaker Narrator
    Quest: Annika's New Job
    Next Objective: Ask Lino about his Neighborhood
    ->END
* {annika_step==1 and have_new_info_annika==false}[Can you remind me what you want me to find?]
    #speaker: Annika
    Please ask Lino what neighborhood he lives in.
    ->END
* {annika_step==1 and have_new_info_annika}[I just talked to Lino; he lives in Orange Grove, but it may be listed as Reagent Village in the brochure.]
    #speaker: Annika
    Ah, I see. Excellent, yes, it's right here on the brochure.
    Thank you, this has all been really useful information.
    #speaker: Narrator
    Annika sighs, her eyes cast downward a little disappointedly.
    #speaker: Annika
    Logistically, I can see myself living here.
    But I'm not sure if this is a spot I'd want to be.
    I wish I knew of something fun happening downtown...
    Somewhere I could meet new friends and engage in the culture of the city.
    Do you think you could find something like that? Maybe a club or something?
    ~helpful+=smallChange
    ~pragmatic+=smallChange
    ~annika+=smallChange
    ~have_new_info_annika=false
    ~annika_step=2
    #speaker Narrator
    Quest: Annika's New Job
    Next Objective: Find an advertisement for an upcoming event.
    ->END
* {annika_step==2 and have_new_info_annika==false}[Can you remind me what you want me to find?]
    #speaker: Annika
    I wish I knew of something fun happening downtown...
    Somewhere I could meet new friends and engage in the culture of the city.
    Do you think you could find something like that? Maybe a club or something?
    ->END
* {annika_step==2 and have_new_info_annika}[Any chance you'd be interested in joining a local band?]
    #speaker: Narrator
    You hand Annika the poster for the Four Gophers' performance.
    #speaker: Annika
    Hmm. The Four Gophers?
    Strange name for a band. But... that sounds like a really nice time. 
    Thank you, {name}, I think I'm starting to feel at home here.
    I'm really glad I was able to meet you!
    ~annika_quest=false
    ~annika_step = 3
    ~annika+=bigChange
    ~lino+=bigChange
    ~friendly+=bigChange
    ~helpful+=bigChange
    ~have_new_info_annika=false
    #speaker Narrator
    Quest: Annika's New Job
    Quest Complete!
    #speaker Narrator
    Quest: The Four Gophers
    Next Objective: Tell Korra the Good News
    ->END
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
* {gophers_step==0}[I'm still looking for a fourth gopher!]
    #speaker: Korra
    Thanks for doing that!
    Best of luck :-)
    ->END
* {gophers_step==1}[Good news! I found you a fourth gopher! Er- Band member!]
    #speaker: Korra
    Really? That's fantastic! 
    I'm looking forward to meeting them at practice.
    Thanks for your help, friend! :-)
    ~korra+=bigChange
    ~helpful+=bigChange
    ~friendly+=bigChange
    ~creative+=bigChange
    ~gophers_step=2
    ~four_gophers=false
    #speaker Narrator
    Quest: The Four Gophers
    Next Objective: Quest Complete!
    ->END
* {gophers_step==2}[Looiking forward to your concert this Friday!]
    You got it! It'll be a blast :-)
    ->END
+ Howdy! Talk to you later.
    #speaker: Korra
    You got it!
    ->END
    
===korra_meet_convo===
#speaker: Unknown
Howdy, there!
Any chance you're around this Friday evening?
+ Pardon? 
    #speaker: Unknown
    Oh, right, I guess I should introduce myself.
    The name is Korra! :-D
    ~met_korra=true
    ->korra_introduce
+ Sure, sounds fun! What's going on?
    #speaker: Unknown
    Oh, wait, I guess I should introduce myself first.
    The name is Korra! :-D
    ~adventurous+=smallChange
    ~friendly+=smallChange
    ~met_korra=true
    ->korra_introduce

==korra_introduce===
#speaker: Korra
Anyways. I'm part of a 3-person alt-folk-punk-rock group. We're the Four Gophers!
+ But... there's three of you?
    ~pragmatic+=smallChange
    #speaker: Korra
    Well, that's our biggest problem.
    Ya see, we really thought we'd have another person by now. :-(
    They don't have to do anything crazy, or anything. We just need someone on tambourine.
    It really gives the whole thing another layer.
    Do you think you could find us a fourth member?
    #speaker Narrator
    Quest: The Four Gophers
    Next Objective: Find a Fourth Member for the Four Gophers
    ~four_gophers=true
    ~gophers_step=0
    ->END
+ Cool, sounds like fun!
    ~adventurous+=smallChange
    #speaker: Korra
    It is!
    ...although, we do have an issue.
    Ya see, we really thought we'd have another person by now. :-(
    They don't have to do anything crazy, or anything. We just need someone on tambourine.
    It really gives the whole thing another layer.
    Do you think you could find us a fourth member?
    Here's a poster you can use.
    #speaker Narrator
    Korra gives you a poster with four gophers holding musical instruments.
    Quest: The Four Gophers
    Next Objective: Find a Fourth Member for the Four Gophers
    ~four_gophers=true
    ~gophers_step=0
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
    -> stevie_quest_convo
    
+ Keep Walking
    -> END
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
    They've got a radius of up to the whole game map.
    ->walkies
* {lino_step==2}[I'm looking for a gift. Do you have anything like that?]
    #speaker: Stevie
    Oh, do I ever - is it for a special someone? 
    Well, whoever it's for, I've got you covered.
    #speaker: Narrator
    Stevie pulls out a trunk from nowhere and opens it up.
    #speaker: Stevie
    You've got three options, my good friend:
    ONE! A baby blue cashmere shawl, as soft as a baby goat's behind.
    TWO! A fountain pen with a polished bronze nib. Comes with jet black ink.
    And THREE! A Philadelphia Chili Cheese Dog - still warm from when I cooked it this morning.
    They all happen to be the same price of - 
    #speaker: Narrator
    Stevie peeks at the money in your hand to gauge what you're holding.
    #speaker: Stevie
    Thirty tickets! What do you say?
    ->gift
* {annika_step==0}[Do you have a bus plan, by any chance?]
    #speaker: Stevie
    Aha, yes! You are in luck!
    I do in fact have a bus plan right here.
    #speaker: Narrator
    Stevie pulls a dark blue brochure from his vest pocket and opens it up to show you...
    A twelve-panel brochure?!
    #speaker: Stevie
    As you can see it is equipped with all of the transport information one could ever need!
    Though, as you can imagine, it costs a pretty penny...
    What do you say to...
    One thousand tickets?
    ->bus_pass
+ Nothing right now, thanks.
    #speaker: Stevie
    Well, come back if you need anything!
    ->END

===walkies===
+ ...game map?
    #speaker: Stevie
    Eh, it's an inside joke.
    Now, normally these puppies would go for twenty tickets or so.
    But I'm in a good mood. So I'll give them to you for free. As a promotional item.
    #speaker: Narrator
    Stevie hands you the walkie-talkies.
    ~has_walkies=true
    ~emily_charlie_step=2
    ~stevie+=3
    #speaker: Stevie
    Just remember to come back to me if you need anything else!
    You won't get a better deal anywhere else.
    #speaker: Narrator
    Quest: Emily and Charlie
    Next Objective: Give Walkie-Talkies to Emily
    ->END
+ ha! That's perfect!
    ~funny+=medChange
    #speaker: Stevie
    I knew you'd get it!
    Now, normally these puppies would go for twenty tickets or so.
    But I'm in a good mood. So I'll give them to you for free. As a promotional item.
    #speaker: Narrator
    Stevie hands you the walkie-talkies.
    ~has_walkies=true
    ~emily_charlie_step=2
    ~stevie+=3
    #speaker: Stevie
    Just remember to come back to me if you need anything else!
    You won't get a better deal anywhere else.
    #speaker: Narrator
    Quest: Emily and Charlie
    Next Objective: Give Walkie-Talkies to Emily
    ->END

->END
===gift===
+ The Shawl.
    #speaker: Stevie
    Ah, a warm gift for a warm soul.
    I almost regret parting with such a fine item.
    But I am sure it will find a good home with you and your friend.
    #speaker: Narrator
    You hand Stevie the money and accept the shawl, folding it nicely as you do so.
    #speaker: Stevie
    Best wishes to you and your friend.
    Remember to come back to me if you need anything else!
    #speaker: Narrator
    Quest: A Gift for Annika
    Next Objective: Give Lino the Gift
    ~lino_gift="shawl"
    ~lino_step=3
    ~friendly+=smallChange
    ~thoughtful+=smallChange
    ~stevie+=3
    ->END
+ The Fountain Pen.
    #speaker: Stevie
    Aha, an intelligential gift for an intelligential folk!
    I will warn you to be careful with this...
    Lest you or your friend get carried away with poetry and become the next big name.
    #speaker: Narrator
    You hand Stevie the money and accept the fountain pen.
    #speaker: Stevie
    There you are! Best wishes with the gift.
    #speaker: Narrator
    Quest: A Gift for Annika
    Next Objective: Give Lino the Gift
    ~lino_step=3
    ~lino_gift="pen"
    ~pragmatic+=smallChange
    ~friendly+=smallChange
    ~stevie+=3
    ->END
+ The Chili Dog!
    #speaker: Stevie
    Excellent choice, my good lady!
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
    Next Objective: Give Lino the Gift
    ~lino_step=3
    ~lino_gift="chili"
    ~funny+=medChange
    ~friendly+=smallChange
    ~stevie +=10
    ->END
+ I'm not sure. I'll think about it and get back to you later.
    #speaker: Stevie
    Understood, understood.
    But don't take too long; these gifts go fast!
    ->END
===bus_pass===
+ One thousand?! That's egregious!
    ->bus_pass_continue
+ I don't have that much...
    ->bus_pass_continue
===bus_pass_continue===
    #speaker: Stevie
    Now, don't panic.
    You don't have to pay that sum all at once!
    I'll simply open a tab for you...
    ...at a steep interest rate, of course.
    What do you say to 72% APR?
    + ...Fine. This currency seems made up, anyways.
        #speaker: Stevie
        A wise decision! Here you are, good madame. 
        #speaker: Narrator
        Stevie hands you the bus pass.
        #speaker: Stevie
        Best wishes!
        ~have_new_info_annika=true
        ~funny+=1
        ->END
    + Umm, I suppose that's alright? (Maybe Annika will pay me back...)
        #speaker: Stevie
        A wise decision! Here you are, good madame. 
        #speaker: Narrator
        Stevie hands you the bus pass.
        #speaker: Stevie
        Best wishes!
        ~have_new_info_annika=true
        ~pragmatic+=1
        ->END
    + That's too much. Sorry, Stevie, maybe next time.
        #speaker: Stevie
        Alright, well, come back if you change your  mind!
        I can't guarantee I'll still have it on hand the next time you need it.
        ->END
        
EXTERNAL addFriend(name)

=== function addFriend(friend_name) ===
// defined function to avoid compile time errors in INK
~ return

    
