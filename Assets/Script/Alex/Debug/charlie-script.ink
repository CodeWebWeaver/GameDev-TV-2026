VAR met_charlie = false
VAR dog_quest = false
VAR emily_charlie_quest = false
VAR emily = 0
VAR charlie = 0



=== charlie_day_one ===

{ met_charlie:
    -> charlie_first_convo
- else:
    -> charlie_meet_convo
}

=== charlie_meet_convo ===

#speaker: ???
Hey, lady!

#narrative
You look for the voice. You don’t see anyone in the street.

#speaker: ???
I’m up here!

#narrative
He appears to be much younger than you — maybe around 12 years old.

#speaker: Charlie
Took you long enough. My name is Charlie!

#speaker: Player
Oh, hi, Charlie.

~ met_charlie = true

-> charlie_first_convo


=== charlie_first_convo ===

 ...
 
+ [What are you doing up there?]
    -> lost_dog_overview

+ [Do I know you?]

    #speaker: Charlie
    Nope! I don’t think so? Unless you’re friends with my mom?

    #speaker: Player
    No, I’m new to the area.

    #speaker: Charlie
    Well then, I guess I wouldn’t know you.
    #speaker: Player
    Hmm, I guess not.

     -> convo_after_knowing_question


+ [Keep walking]
    -> END

=== convo_after_knowing_question ===

+ [What are you doing up there?]
    -> lost_dog_overview

+ [Keep walking]
    -> END

=== lost_dog_overview ===

#speaker: Charlie
Hmm? Oh. Right.

#narrative
Charlie sighs.

#speaker: Charlie
I was grounded for staying out too late last night.

#speaker: Charlie
But it’s not my fault!

#speaker: Charlie
My neighbor’s dog got loose, and I was just trying to track it down…

#speaker: Charlie
I would’ve stayed out even later than that, but Mom was already so mad…

#speaker: Charlie
She wouldn’t even listen to my reason for staying out!

+ [That’s not fair at all. Maybe I can talk to your mom and explain the situation.]

    ~ changePersonality("ambitious", 1)
    ~ changePersonality("easygoing", 1)
    ~ charlie += 1
    ~ emily += 1
    ~ emily_charlie_quest = true

    #speaker: Charlie
    You would do that for me?

    #speaker: Charlie
    Hmm… maybe she *would* listen to another adult…

    #speaker: Charlie
    Well, thanks! You should be able to find her somewhere on the street.

    #speaker: Charlie
    I think her name is Beverly? I’m not supposed to call her that, though…

    #narrative
    Quest started: Unite Beverly and Charlie

    #narrative
    Next Objective: Find Beverly

    -> END


+ [Oh no, your neighbor’s dog is missing? I can help look for it.]
    ~ changePersonality("adventurous", 1)
    ~ changePersonality("helpful", 1)
    ~ dog_quest = true
    ~ charlie += 1
    ~ beverly += 1

    #speaker: Charlie
    Thank you! He’s gray, fluffy, and should still be somewhere in the neighborhood.

    #speaker: Charlie
    My neighbor who lost the dog lives down the street. My mom calls her Emily.

    #speaker: Charlie
    She should be able to tell you more.

    #narrative
    Quest started: The Lost Dog

    #narrative
    Next Objective: Find Emily

    -> END


+ [That’s too bad. Good luck with that. I hope everything works out.]
 ~ changePersonality("helpful", -1)
  ~ changePersonality("charlie", -1)
   ~ changePersonality("beverly", -1)

    #speaker: Charlie
    Thanks anyway.

    -> END