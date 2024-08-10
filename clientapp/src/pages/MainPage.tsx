import { useContext, useEffect, useState } from "react";
import Notes from "../components/Notes";
import { notesGet } from "../requests/notesRequests";
import Note from "../classes/Note";
import { authContext } from "../contexts";
import { Link } from "react-router-dom";
import { InputComponent } from "../components/InputComponent";
import NoteCreateModalWindow from "../components/NoteCreateModalWindow";
import "../styles/MainPage.css";

export default function MainPage() {
  const authCon = useContext(authContext);
  const [notes, setNotes] = useState<Note[]>([]);

  const [sortingType, setSortingType] = useState("dateDesc");
  const [search, setSearch] = useState("");
  const [from, setFrom] = useState(0);

  const defaultPagin: number = 10;
  const [totalNotes, setTotalNotes] = useState(0);
  const [pagin, setPagin] = useState(defaultPagin);

  async function getNotesQuery() {
    let fromWithPagin = from + pagin;
    let toParam =
      totalNotes != 0 && fromWithPagin >= totalNotes
        ? totalNotes
        : fromWithPagin;
    try {
      const res = await notesGet({
        from: from,
        to: fromWithPagin,
        sortType: sortingType,
        search: search,
      });
      console.log("From: ", from);
      console.log("To: ", toParam);
      if (res.status == 200) {
        setNotes(res.data.notes);
        setTotalNotes(res.data.totalCount);
      }
    } catch (error) {}
  }
  useEffect(() => {
    getNotesQuery();
  }, [from, pagin, sortingType, search]);

  function pagination(i: number) {
    setFrom(from + pagin * i);
  }

  return authCon.isAuthenticated ? (
    <>
      <div>
        <NoteCreateModalWindow
          notesListChangeFun={async () => await getNotesQuery()}
        />
      </div>
      <div className="totalNotes">
        <InputComponent
          id="paginInput"
          inputType="number"
          labelText="The number of displayed tasks: "
          beforeValidationFun={(e: any) =>
            setPagin(parseInt(e.currentTarget.value))
          }
          inputOtherProps={{
            className: "pagin-input",
            min: 3,
            max: 15,
            defaultValue: defaultPagin,
          }}
        />
      </div>
      <div>
        <span>Search: </span>
        <input
          id="searchText"
          type="text"
          onChange={(e) => setSearch(e.target.value)}
          className={"custom-input"}
        />
      </div>
      <div>
        <select
          name="sortingTypes"
          id="sortingTypes"
          onChange={(e) => setSortingType(e.target.value)}
        >
          <option value={"dateDesc"}>New ones first.</option>
          <option value={"date"}>The old ones first</option>
        </select>
      </div>
      <div>
        {notes != null ? (
          <Notes
            noteList={notes}
            ulChildren={""}
            liChildren={{ class: "note" }}
            notesListChangeFun={async () => await getNotesQuery()}
          />
        ) : (
          <p>You don't have any notes</p>
        )}
      </div>
      <div className="otherPages">
        {from - pagin >= 0 ? (
          <button className={"custom-input"} onClick={() => pagination(-1)}>
            Prev
          </button>
        ) : (
          <></>
        )}
        {from + pagin < totalNotes ? (
          <button className={"custom-input"} onClick={() => pagination(1)}>
            Next
          </button>
        ) : (
          <></>
        )}
        {from + pagin * 2 < totalNotes ? (
          <button className={"custom-input"} onClick={() => pagination(2)}>
            Next x2
          </button>
        ) : (
          <></>
        )}
      </div>
    </>
  ) : (
    <h2>
      <Link to={"/registration"}>Register</Link> or{" "}
      <Link to={"/login"}>log in</Link> to your account to use our application
    </h2>
  );
}
