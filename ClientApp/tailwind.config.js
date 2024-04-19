/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,ts}", "./node_modules/flowbite/**/*.js"],
  theme: {
    extend: {
      width: {
        1440: "1440px",
        135: "135.484px",
        480: "480px",
        397: "397px",
        416: "416px",
        390: "390px",
      },
      height: {
        1024: "1024px",
        62: "62px",
        844: "844px",
      },
      margin: {
        65: "65px",
      },
      borderRadius: {
        xb1: "15px",
        xb2: "14px",
      },
      borderWidth: {
        borderkhoa: "1px solid var(--secondary-200, #EBEAED)",
      },
      backgroundColor: {
        logo: "lightgray 50% / cover no-repeat",
        field: "var(--primary-0, #FFF)",
        button: "#1882E4",
        buttonkhoa: "var(--blue-blue-500, #3B82F6)",
      },
      boxShadow: {
        box1: "0px 34px 114px 0px rgba(0, 0, 0, 0.08)",
      },
      fontFamily: {
        chu: "Plus Jakarta Sans",
        chu2: "Manrope",
        chu3: "Inter",
      },
      fontSize: {
        32: "32px",
        chung: "12px",
        ql: "16px",
        btn: "14px",
      },
      lineHeight: {
        48: "48px",
      },
      letterSpacing: {
        96: "-0.96px",
        24: "-0.24px",
      },
      textColor: {
        fields: "var(--secondary-300, #ACB5BB)",
        fields1: "#1882E4",
        fields2: "#DCE4E8",
        fieldskhoa: "var(--fill-dark-main-dark, #2E2C34)",
        fieldsDS:
          "var(--light-text-default-text, var(--blue-gray-blue-gray-800, #1E293B))",
        fieldsCN:
          " var(--light-text-primary-text, var(--blue-blue-500, #3B82F6))",
      },
      padding: {
        xx2: "27px 24px",
        xx3: "12px 24px",
      },
      gap: {
        2: "2px",
        x10: "10px",
      },
    },
  },
  plugins: [require("flowbite/plugin")],
};
