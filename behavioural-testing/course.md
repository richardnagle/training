class: centre, middle
# Behavioural Unit Testing

---
class: two-column
#Intro
.column[
**Interaction**
* Mockist style
* London school
* Tests interaction/collaberation
* Many mocks*
* Little test data
* Test fixture per class
]
.column[
<br>
**Behavioural**
* Classic style
* Chicago/Detroit school
* Tests state (input/output)
* Few mocks*
* Much test data
* Test fixture per state (ish)
]
\*We'll use the term *mock* to cover all types of doubles - stubs, mocks, fakes, spyies, etc.
---
#Case study
We work for a publishing house developing a *Reviews-API* which receives book reviews and stores them.
The request is in an industry standard API...
```http
Content-type: application/json
Referer: https://literaryreview.co.uk/
```
```json
{
    "isbn": "9788175257665",
    "title": "War and Peace",
    "author": "Leo Tolstoy",
    "reviewer": "Karen Castle",
    "sections": [
        {
            "name": "Title",
            "text": "A Classic of our Times"
        },
        {
            "name": "SubTitle",
            "text": "Karen Castle reviews Tolstoy's latest work"
        },
        {
            "name": "Body",
            "text": "Blah blah blah"
        }
    ]
}
```
---
#Requirements
When a review is received it will be validated. The table below shows the validation
and which status code and message to respond with on failure.

| Validation                                    | Status Code    | Error Message                   |
|-----------------------------------------------|----------------|---------------------------------|
| The `Content-type` must be `application/json` | 415            | Incorrect content type          |
| The `Referer` must have a valid URI format    | 400            | Bad referer uri                 |
| The `isbn` must be 13 numeric characters      | 400            | Invalid ISBN                    |

---
#Requirements (cont.)
A valid review will be saved to our database using the `ISaveReviews` interface which has the following signature:

```c#
public interface ISaveReviews
{
    void Insert(ReviewDto dto);
}

public class ReviewDto
{
    public long ISBN { get; set; }
    public string Reviewer { get; set; }
    public string Uri { get; set; }
    public string Text { get; set; }
}
```

The `Text` property shall be formatted as follows:

```html
&lt;h1&gt;Title&lt;/h1&gt;
&lt;h2&gt;SubTitle&lt;/h2&gt;
&lt;p&gt;Body&lt;/p&gt;
```

After the save a status code 201 is sent with no error message.

---
#Testing in a Mockist fashion
Firstly I need to decide what my dependencies are. Let's write down what I need to do...

.written-text[
Validate the http headers

Validate the body

Walk the sections

Map the http headers to the ReviewDto

Map the body to the ReviewDto

Format the section to html

Map the html to ReviewDto.Text
]
---
# Deciding on my Dependencies
.written-text[
.red-text[Validate] the http headers

.red-text[Validate] the body

.blue-text[Walk] the sections

.green-text[Map] the http headers to the ReviewDto

.green-text[Map] the body to the ReviewDto

.purple-text[Format] the section to html

.green-text[Map] the html to ReviewDto.Text
]

Therefore
.written-text[
public interface .red-text[IValidator];

public interface .blue-text[ISectionWalker];

public interface .green-text[IReviewDtoMapper];

public interface .purple-text[IHtmlFormatter];
]

---
#Good example
Only mock persistence
Use builder
---
#What I don't like about mockist example
* Test complexity
* Tests do not demonstrate behaviour
* Tight coupling to code
* Difficult to re factor
* Too many tests
* Testing incidentals (e.g. Mapper)
---
#What's worse about behavioural testing
* Test case organisation
    * Fix with feature folder
    * Fix with coverage tool
* Tight coupling to constructors
    * Fix with builders
* Cascading test failures
    * No fix but is this such a bad thing?
---
#But, Hang-On, you used a Mock
When should I use a mock

*Rarely*

No really

*Yes, really. But if you must have a list....*

* When something is slow
    * Database
    * Remote call
* When something is difficult to set-up
    * Database
    * Statics (`DateTime.Now`)
    * Third-party code
* When something is unreliable
    * Remote call
* When behaviour is very complex (very rare)
---
#That's only half the story.
All I did was take a sut designed in mocks style and improve tests to a behavioural style
TDD is about design at least as much as testing
How you go about TDD has a direct effect on the design if your code
If you mostly test interactions, you'll get more interactions.
If you mostly test behaviours, you'll get more behaviours.
Now start from scratch and do TDD in fully behavioural style.
---
#Further reading
* **Ron Jeffries**<br>
    * *Thoughts on Mocks*<br>
        [http://ronjeffries.com/articles/015-11/tdd-mocks/](http://ronjeffries.com/articles/015-11/tdd-mocks/)

* **Jason Gorman**<br>
    * *Classical TDD or "London School"*<br>
        [http://codemanship.co.uk/parlezuml/blog/?postid=987](http://codemanship.co.uk/parlezuml/blog/?postid=987)

* **Ken Scambler**<br>
    * *To Kill a Mockingtest*<br>
        [http://rea.tech/to-kill-a-mockingtest/](http://rea.tech/to-kill-a-mockingtest/)<br><br>
    * *Imagine a World without Mocks*<br>
        [https://www.slideshare.net/kenbot/imagine-a-world-without-mocks](https://www.slideshare.net/kenbot/imagine-a-world-without-mocks)
