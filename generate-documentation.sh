rm docs/PDFs/*
pandoc README.md -o docs/PDFs/README.pdf
pushd docs
for f in *.md; do pandoc "$f" -s -o "PDFs/${f%.md}.pdf"; done
popd
