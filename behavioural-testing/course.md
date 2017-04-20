class: centre, middle
# Behavioural Unit Testing

---
class: two-column
#Intro
.column[
**Behavioural**
* Classical
* Chicago / Detroit
]
.column[
**Interaction**
* Mockist
* London
]

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
#What's don't I like about 1st example
* Test readability
* Tight coupling to code
* Difficult to re factor
* Too many tests
---
#What's worse about behavioural testing
* Test case organisation
    * Fix with feature folder
    * Fix with coverage tool
* Cascading test failures
    * No fix but is this such a bad thing?
* Tight coupling to constructors
    * Fix with builders
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
---