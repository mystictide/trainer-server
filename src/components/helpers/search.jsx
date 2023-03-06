import { useMemo, useState } from "react";
import { useSelector } from "react-redux";
import Select from "react-select";

function Search({ setFilter, setKeyword, keyword }) {
  const { filteredData, cats } = useSelector((state) => state.cms);
  const categoryOptions = useMemo(() => cats, []);

  const [category, setCategories] = useState(
    filteredData.filterModel.Category ? filteredData.filterModel.Category : ""
  );

  const [filterModel, setFilterModel] = useState({
    category: category,
  });

  const onCategoryChange = (value) => {
    setCategories(value);
    setFilterModel((prevState) => ({
      ...prevState,
      category: value,
    }));
  };

  return (
    <div className="v-items c-gap-10 r-gap-10">
      <label>Categories</label>
      <Select
        className="select"
        id="category"
        name="category"
        placeholder={"select categories"}
        options={categoryOptions}
        getOptionLabel={(options) => options["Name"]}
        getOptionValue={(options) => options["ID"]}
        value={category}
        onChange={onCategoryChange}
      />
      {/* <input
        type="text"
        id="keyword"
        name="keyword"
        value={keyword}
        placeholder={"search by name.."}
        className="main-border"
        onChange={(e) => setKeyword(e.target.value)}
      /> */}
      <button
        className="btn-function"
        onClick={(e) => setFilter(e, 1, filterModel)}
      >
        Search
      </button>
    </div>
  );
}

export default Search;
