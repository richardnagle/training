class: centre, middle
# Test Driven Development & Design

---
class: two-column
#TDD Styles: Testing
.column[
**Classicist**
* aka State-Based
* aka Chicago/Detroit School
* Tests state (input/output)
* Few mocks
* Much test data
* Test fixture per state
<br><br>
![Book Cover of Test Driven Development by Example - Beck](images/tdd_by_example_cover.jpg)
]
.column[
<br>
**Mockist**
* aka Interaction
* aka London School
* Tests interaction/collaberation
* Many mocks
* Little test data
* Test fixture per class
<br><br>
![Book Cover of Growing Object-Oriented Software, Guided by Tests - Freeman & Pryce](images/goos_cover.jpg)
]
---
class: two-column
#TDD Styles: Implementation
.column[
**Inside-out**
* Start with a unit test
* Components are built in any<br>order
* Components are integrated<br>at end
* Acceptance testing optional
]
.column[
<br>
**Outside-in**
* Start with an acceptance test
* Components built from outside (UI)
* Components are integrated incrementally
<br><br>
* Acceptance testing mandatory
]
---
#Testing-Style Quadrants
<table class="quadrants">
    <tr>
        <td class="titles">
            <div>Inside-out</div>
        </td>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td class="titles">
            <div>Outside-in</div>
        </td>
        <td></td>
        <td></td>
    </tr>
    <tr class="titles">
        <td class="titles"></td>
        <td>Mockist</td>
        <td>Classicist</td>
    </tr>
</table>

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
#Inside-Out Mockist Testing
<table class="quadrants">
    <tr>
        <td class="titles">
            <div>Inside-out</div>
        </td>
        <td class="selected"></td>
        <td></td>
    </tr>
    <tr>
        <td class="titles">
            <div>Outside-in</div>
        </td>
        <td></td>
        <td></td>
    </tr>
    <tr class="titles">
        <td class="titles"></td>
        <td>Mockist</td>
        <td>Classicist</td>
    </tr>
</table>
---
#Getting Started
Firstly I need to decide what to build. Let's write down what I need to do...

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
# Deciding on my Components
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
public interface .red-text[IReviewValidator];

public interface .blue-text[ISectionWalker];

public interface .green-text[IReviewDtoMapper];

public interface .purple-text[IReviewHtmlFormatter];
]
---
#Conclusions: Inside-Out Mockist Testing
<table class="quadrants">
    <tr>
        <td class="titles">
            <div>Inside-out</div>
        </td>
        <td class="selected">
            <ul class="conclusions">
                <li><span class="tick"></span><text>Loosely coupled design</text></li>
                <li><span class="tick"></span><text>Test autonomy</text></li>
                <li><span class="cross"></span><text>Poor abstractions</text></li>
                <li><span class="cross"></span><text>Difficult to refactor</text></li>
                <li><span class="cross"></span><text>Tests tightly coupled to code</text></li>
                <li><span class="cross"></span><text>Tests do not demonstrate behaviour</text></li>
                <li><span class="cross"></span><text>Testing incidentals (e.g. Mapper)</text></li>
            </ul>
        </td>
        <td></td>
    </tr>
    <tr>
        <td class="titles">
            <div>Outside-in</div>
        </td>
        <td></td>
        <td></td>
    </tr>
    <tr class="titles">
        <td class="titles"></td>
        <td>Mockist</td>
        <td>Classicist</td>
    </tr>
</table>
---
#Inside-Out Classicist Testing
<table class="quadrants">
    <tr>
        <td class="titles">
            <div>Inside-out</div>
        </td>
        <td></td>
        <td class="selected"></td>
    </tr>
    <tr>
        <td class="titles">
            <div>Outside-in</div>
        </td>
        <td></td>
        <td></td>
    </tr>
    <tr class="titles">
        <td class="titles"></td>
        <td>Mockist</td>
        <td>Classicist</td>
    </tr>
</table>
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
#Creating Test Builders
* Create a private field for each piece of data you want to set
* In the constructor, set the value of each private field to a valid value for the most common use-case
* Add a `With` method to set the value for each private field; at the end `return this` to create fluent interface
* Add a `Build` method which creates a new instance and populates it from private field values
* No conditional logic in `With` or `Build` methods
* Compose builders together using `With` methods
```c#
    .WithCustomer(new CustomerBuilder().WithName("foobar"))
```
or
```c#
    .WithCustomer(cust => cust.WithName("foobar"))
```
---
#Conclusions: Inside-Out Classicist Testing
<table class="quadrants">
    <tr>
        <td class="titles">
            <div>Inside-out</div>
        </td>
        <td class="not-selected">
            <ul class="conclusions">
                <li><span class="tick"></span><text>Loosely coupled design</text></li>
                <li><span class="tick"></span><text>Test autonomy</text></li>
                <li><span class="cross"></span><text>Poor abstractions</text></li>
                <li><span class="cross"></span><text>Difficult to refactor</text></li>
                <li><span class="cross"></span><text>Tests tightly coupled to code</text></li>
                <li><span class="cross"></span><text>Tests do not demonstrate behaviour</text></li>
                <li><span class="cross"></span><text>Testing incidentals (e.g. Mapper)</text></li>
            </ul>
        </td>
        <td class="selected">
            <ul class="conclusions">
                <li><span class="tick"></span><text>More readable tests (with builders)</text></li>
                <li><span class="tick"></span><text>Better to refactor, but still difficult</text></li>
                <li><span class="cross"></span><text>Tests less autonomous</text></li>
                <li><span class="cross"></span><text>Tests tightly coupled to code</text></li>
                <li><span class="cross"></span><text>Tests do not demonstrate behaviour</text></li>
                <li><span class="cross"></span><text>Testing incidentals (e.g. Mapper)</text></li>
                <li><span class="cross"></span><text>Duplication of testing</text></li>
            </ul>
        </td>
    </tr>
    <tr>
        <td class="titles">
            <div>Outside-in</div>
        </td>
        <td></td>
        <td></td>
    </tr>
    <tr class="titles">
        <td class="titles"></td>
        <td>Mockist</td>
        <td>Classicist</td>
    </tr>
</table>
---
#Classicist Code
```c#
    public Response Handle(Request<PostedReview> request)
    {
        if (!_reviewValidator.ValidateContentType(request))
            return new Response(415, "Incorrect content type");

        if (!_reviewValidator.ValidateReferer(request))
            return new Response(400, "Bad referer uri");

        if (!_reviewValidator.ValidateISBN(request))
            return new Response(400, "Invalid ISBN");

        var title = _sectionWalker.GetText(request.Body.Sections, "title");
        var subTitle = _sectionWalker.GetText(request.Body.Sections, "subtitle");
        var body = _sectionWalker.GetText(request.Body.Sections, "body");
        var bodyHtml = _htmlFormatter.Format(title, subTitle, body);

        var dto = new ReviewDto();

        _dtoMapper.MapBodyFields(request.Body, dto);
        _dtoMapper.MapText(bodyHtml, dto);
        _dtoMapper.MapHttpHeaders(request.Headers, dto);

        _database.Insert(dto);

        return new Response(201,"");
    }
```
---
#Mockist Code
```c#
    public Response Handle(Request<PostedReview> request)
    {
        if (!_validator.ValidateContentType(request))
            return new Response(415, "Incorrect content type");

        if (!_validator.ValidateReferer(request))
            return new Response(400, "Bad referer uri");

        if (!_validator.ValidateISBN(request))
            return new Response(400, "Invalid ISBN");

        var reviewDto = new ReviewDto();

        var title = _sectionWalker.GetText(request.Body.Sections, "Title");
        var subTitle = _sectionWalker.GetText(request.Body.Sections, "SubTitle");
        var body = _sectionWalker.GetText(request.Body.Sections, "Body");

        var text = _htmlFormatter.Format(title, subTitle, body);

        _mapper.MapHttpHeaders(request.Headers, reviewDto);
        _mapper.MapBodyFields(request.Body, reviewDto);
        _mapper.MapText(text, reviewDto);

        _databaseService.Insert(reviewDto);

        return new Response(201, "");
    }
```
---
#Outside-In Mockist Testing
<table class="quadrants">
    <tr>
        <td class="titles">
            <div>Inside-out</div>
        </td>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td class="titles">
            <div>Outside-in</div>
        </td>
        <td class="selected"></td>
        <td></td>
    </tr>
    <tr class="titles">
        <td class="titles"></td>
        <td>Mockist</td>
        <td>Classicist</td>
    </tr>
</table>
---
#Conclusions: Outside-In Mockist Testing
<table class="quadrants">
    <tr>
        <td class="titles">
            <div>Inside-out</div>
        </td>
        <td class="not-selected">
            <ul class="conclusions">
                <li><span class="tick"></span><text>Loosely coupled design</text></li>
                <li><span class="tick"></span><text>Test autonomy</text></li>
                <li><span class="cross"></span><text>Poor abstractions</text></li>
                <li><span class="cross"></span><text>Difficult to refactor</text></li>
                <li><span class="cross"></span><text>Tests tightly coupled to code</text></li>
                <li><span class="cross"></span><text>Tests do not demonstrate behaviour</text></li>
                <li><span class="cross"></span><text>Testing incidentals (e.g. Mapper)</text></li>
            </ul>
        </td>
        <td class="not-selected">
            <ul class="conclusions">
                <li><span class="tick"></span><text>More readable tests (with builders)</text></li>
                <li><span class="tick"></span><text>Better to refactor, but still difficult</text></li>
                <li><span class="cross"></span><text>Tests less autonomous</text></li>
                <li><span class="cross"></span><text>Tests tightly coupled to code</text></li>
                <li><span class="cross"></span><text>Tests do not demonstrate behaviour</text></li>
                <li><span class="cross"></span><text>Testing incidentals (e.g. Mapper)</text></li>
                <li><span class="cross"></span><text>Duplication of testing</text></li>
            </ul>
        </td>
    </tr>
    <tr>
        <td class="titles">
            <div>Outside-in</div>
        </td>
        <td class="selected">
            <ul class="conclusions">
                <li><span class="tick"></span><text>Tests less coupled to code</text></li>
                <li><span class="tick"></span><text>Test autonomy</text></li>
                <li><span class="tick"></span><text>Easier to refactor</text></li>
                <li><span class="tick"></span><text>Tests focus on core elements</text></li>
                <li><span class="tick"></span><text>Clean uncoupled implementation</text></li>
                <li><span class="cross"></span><text>Tests do not demonstrate behaviour</text></li>
                <li><span class="cross"></span><text>Too many tests</text></li>
                <li><span class="cross"></span><text>Difficult to implement (over-engineered)</text></li>
            </ul>
        </td>
        <td></td>
    </tr>
    <tr class="titles">
        <td class="titles"></td>
        <td>Mockist</td>
        <td>Classicist</td>
    </tr>
</table>
---

#Outside-In Classicist Testing
<table class="quadrants">
    <tr>
        <td class="titles">
            <div>Inside-out</div>
        </td>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td class="titles">
            <div>Outside-in</div>
        </td>
        <td></td>
        <td class="selected"></td>
    </tr>
    <tr class="titles">
        <td class="titles"></td>
        <td>Mockist</td>
        <td>Classicist</td>
    </tr>
</table>
---

#Conclusions: Outside-In Classicist Testing
<table class="quadrants">
    <tr>
        <td class="titles">
            <div>Inside-out</div>
        </td>
        <td class="not-selected">
            <ul class="conclusions">
                <li><span class="tick"></span><text>Loosely coupled design</text></li>
                <li><span class="tick"></span><text>Test autonomy</text></li>
                <li><span class="cross"></span><text>Poor abstractions</text></li>
                <li><span class="cross"></span><text>Difficult to refactor</text></li>
                <li><span class="cross"></span><text>Tests tightly coupled to code</text></li>
                <li><span class="cross"></span><text>Tests do not demonstrate behaviour</text></li>
                <li><span class="cross"></span><text>Testing incidentals (e.g. Mapper)</text></li>
            </ul>
        </td>
        <td class="not-selected">
            <ul class="conclusions">
                <li><span class="tick"></span><text>More readable tests (with builders)</text></li>
                <li><span class="tick"></span><text>Better to refactor, but still difficult</text></li>
                <li><span class="cross"></span><text>Tests less autonomous</text></li>
                <li><span class="cross"></span><text>Tests tightly coupled to code</text></li>
                <li><span class="cross"></span><text>Tests do not demonstrate behaviour</text></li>
                <li><span class="cross"></span><text>Testing incidentals (e.g. Mapper)</text></li>
                <li><span class="cross"></span><text>Duplication of testing</text></li>
            </ul>
        </td>
    </tr>
    <tr>
        <td class="titles">
            <div>Outside-in</div>
        </td>
        <td class="not-selected">
            <ul class="conclusions">
                <li><span class="tick"></span><text>Tests less coupled to code</text></li>
                <li><span class="tick"></span><text>Test autonomy</text></li>
                <li><span class="tick"></span><text>Easier to refactor</text></li>
                <li><span class="tick"></span><text>Tests focus on core elements</text></li>
                <li><span class="tick"></span><text>Clean uncoupled implementation</text></li>
                <li><span class="cross"></span><text>Tests do not demonstrate behaviour</text></li>
                <li><span class="cross"></span><text>Too many tests</text></li>
                <li><span class="cross"></span><text>Difficult to implement (over-engineered)</text></li>
            </ul>
        </td>
        <td class="selected">
            <ul class="conclusions">
                <li><span class="tick"></span><text>Low test coupling to code</text></li>
                <li><span class="tick"></span><text>Easiest to refactor</text></li>
                <li><span class="tick"></span><text>Tests demonstrate behaviour</text></li>
                <li><span class="tick"></span><text>Clean implementation</text></li>
                <li><span class="tick"></span><text>Good abstractions</text></li>
                <li><span class="tick"></span><text>Least number of tests</text></li>
                <li><span class="cross"></span><text>Poor test autonomy</text></li>
                <li><span class="cross"></span><text>Design is more coupled</text></li>
            </ul>
        </td>
    </tr>
    <tr class="titles">
        <td class="titles"></td>
        <td>Mockist</td>
        <td>Classicist</td>
    </tr>
</table>

---
#Do You Remember This Slide
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
public interface .red-text[IReviewValidator];

public interface .blue-text[ISectionWalker];

public interface .green-text[IReviewDtoMapper];

public interface .purple-text[IReviewHtmlFormatter];
]
---
#Concentrate on Nouns instead of Verbs
.written-text[
Validate the .red-text[http headers]

Validate the .blue-text[body]

Walk the .green-text[sections]

Map the .red-text[http headers] to the ReviewDto

Map the .blue-text[body] to the ReviewDto

Format the .green-text[section] to html

Map the .blue-text[html] to ReviewDto.Text
]
Therefore
.written-text[
public class .red-text[HttpHeaders];

public class .blue-text[HtmlBody];

public class .green-text[Section];
]
---
#Name Classes after what they are, not what they do
---
#The Four Elements of Simple Design
In XP, Kent Beck defined Simple Design as
    1. Runs all the tests
    2. Has no duplicated logic
    3. States every intention important to the programmer
    4. Has the fewest possible classes and methods
The order is important

Over time this has become
    1. Passes the tests
    2= No duplication
    2= Reveals intent
    3. Fewest elements
---
#Further reading I
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
#Further reading II
* **Martin Fowler**<br>
    * *Beck Design Rules*<br>
        [https://martinfowler.com/bliki/BeckDesignRules.html](https://martinfowler.com/bliki/BeckDesignRules.html)

* **J. B. Rainsberger (JBrains)**<br>
    * *The Four Elements of Simple Design*<br>
        [http://blog.jbrains.ca/permalink/the-four-elements-of-simple-design](http://blog.jbrains.ca/permalink/the-four-elements-of-simple-design)
    * *Putting An Age-Old Battle To Rest*<br>
        [http://blog.thecodewhisperer.com/permalink/putting-an-age-old-battle-to-rest](http://blog.thecodewhisperer.com/permalink/putting-an-age-old-battle-to-rest)