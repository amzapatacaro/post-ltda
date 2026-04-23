#!/usr/bin/env bash
set -euo pipefail

resolve_sqlcmd() {
  for c in /opt/mssql-tools18/bin/sqlcmd /opt/mssql-tools/bin/sqlcmd; do
    if [ -x "$c" ]; then
      echo "$c"
      return 0
    fi
  done
  echo "sqlcmd not found" >&2
  return 1
}

SQLCMD="$(resolve_sqlcmd)"
DB_HOST="${DB_HOST:-db}"
PASSWORD="${MSSQL_SA_PASSWORD:?MSSQL_SA_PASSWORD is required}"

cnt_raw="$("$SQLCMD" -S "$DB_HOST" -U sa -P "$PASSWORD" -C -Q "SET NOCOUNT ON; SELECT COUNT(*) FROM sys.databases WHERE name = N'JujuTest'" -h -1 -W 2>/dev/null | tr -d '\r' || true)"
cnt="$(echo "$cnt_raw" | grep -Eo '[0-9]+' | tail -1 || echo 0)"

if [ "${cnt:-0}" -ge 1 ]; then
  echo "Database JujuTest already exists; skipping seed script."
  exit 0
fi

echo "Applying Seeds/JujuTests.Script.sql..."
exec "$SQLCMD" -S "$DB_HOST" -U sa -P "$PASSWORD" -C -b -i /init.sql
