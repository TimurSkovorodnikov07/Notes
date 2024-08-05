import { useContext, useEffect, useState } from "react";
import Notes from "../components/Notes";
import NoteCreate from "../components/NoteCreate";
import { notesGet } from "../requests/notesRequests";
import Note from "../classes/Note";
import { authContext } from "../contexts";
import { Link } from "react-router-dom";
import { InputComponent } from "../components/InputComponent";

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
    try {
      const res = await notesGet({
        from: from,
        to: from + totalNotes,
        sortType: sortingType,
        search: search,
      });
      if (res?.status === 200) {
        setNotes(res?.data);
        setTotalNotes(res?.headers["selectedNotesTotalCount"]);
      }
    } catch (error) {}
  }
  useEffect(() => {
    getNotesQuery();
  }, [from, totalNotes, sortingType, search]);

  function pagination(i: number) {
    setFrom(from + pagin * i);
  }

  return authCon.authenticated ? (
    <>
      <div>
        <NoteCreate notesListChangeFun={async () => await getNotesQuery()} />
      </div>
      <div className="totalNotes">
        <InputComponent
          id="totalNotesButton"
          inputType="number"
          labelText="The number of displayed tasks:"
          inputOtherProps={{
            min: 3,
            defaultValue: defaultPagin,
            onClick: (e: any) => setPagin(parseInt(e.currentTarget.value)),
          }}
        />
      </div>
      <div>
        <span>Search: </span>
        <input
          id="searchText"
          type="text"
          onChange={(e) => setSearch(e.target.value)}
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
      <div></div>
      <div>
        {notes != null ? (
          <Notes
            noteList={notes}
            ulChildren={""}
            liChildren={""}
            notesListChangeFun={async () => await getNotesQuery()}
          />
        ) : (
          <p>You don't have any notes</p>
        )}
      </div>
      <div className="otherPages">
        {from - pagin > 0 ? (
          <button onClick={() => pagination(-1)}>Prev</button>
        ) : (
          <></>
        )}
        {from - pagin > 0 ? (
          <button onClick={() => pagination(1)}>Next</button>
        ) : (
          <></>
        )}
        {from - pagin > 0 ? (
          <button onClick={() => pagination(2)}>Next x2</button>
        ) : (
          <></>
        )}
        {from - pagin > 0 ? (
          <button onClick={() => pagination(3)}>Next x3</button>
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
