(() => {
    "use strict";

    const playbackCache = new WeakMap();

    function getPlaybackData(container) {
        if (playbackCache.has(container)) {
            return playbackCache.get(container);
        }

        const dataElement = container.querySelector("[data-playback-data]");

        if (!dataElement) {
            return null;
        }

        try {
            const plays = JSON.parse(dataElement.textContent);

            const data = {
                plays,
                index: Number(container.dataset.initialIndex || 0),
                lastSituation: null
            };

            playbackCache.set(container, data);

            return data;
        } catch (error) {
            console.error("Football Playback Data is Invalid:", error);

            return null;
        }
    }

    function setText(root, selector, value) {
        const element = root.querySelector(selector);

        if (element) {
            element.textContent = value ?? "-";
        }
    }

    function setVisible(element, visible) {
        if (element) {
            element.style.display = visible ? "" : "none";
        }
    }

    function clamp(value, min, max) {
        return Math.min(max, Math.max(min, value));
    }

    function getPossessionName(field, possessionId) {
        if (!possessionId) {
            return "Unknown";
        }

        if (String(possessionId) === field.dataset.awayTeam) {
            return field.dataset.awayAbbreviation;
        }

        if (String(possessionId) === field.dataset.homeTeam) {
            return field.dataset.homeAbbreviation;
        }

        return "Unknown";
    }

    function updateField(field, situation) {
        const ball = field.querySelector("[data-football-ball]");
        const gain = field.querySelector("[data-football-gain]");
        const redZone = field.querySelector("[data-football-redzone]");

        const possession = situation?.possession ?? situation?.possessionTeamId;

        const normalizeId = value => String(value ?? "").trim().toLowerCase();

        const possessionId = normalizeId(possession);
        const awayId = normalizeId(field.dataset.awayTeam);
        const homeId = normalizeId(field.dataset.homeTeam);

        const isAway = possessionId !== "" && possessionId === awayId;
        const isHome = possessionId !== "" && possessionId === homeId;

        const yardsToGoal = Number(situation?.yardsToEndzone);

        if (!situation || situation.yardsToEndzone == null || !Number.isFinite(yardsToGoal)) {
            setVisible(ball, false);
            setVisible(gain, false);
            setVisible(redZone, false);

            setText(field, "[data-football-possession]", "-");
            setText(field, "[data-football-down]", "-");
            setText(field, "[data-football-yards]", "-");

            return;
        }

        if (!isAway && !isHome) {
            console.warn("Unable to Determine Possession Team:", {
                possessionId,
                awayId,
                homeId
            });

            setVisible(ball, false);
            setVisible(gain, false);

            return;
        }

        const attacksRight = isAway;

        const position = clamp(attacksRight ? 100 - yardsToGoal : yardsToGoal, 0, 100);

        let gainPosition = null;

        if (situation.distance != null) {
            const distance = Number(situation.distance);

            if (Number.isFinite(distance)) {
                const yardsToGain = distance === 0 ? yardsToGoal : distance;

                gainPosition = clamp(position + (attacksRight ? yardsToGain : -yardsToGain), 0, 100);
            }
        }

        if (ball) {
            ball.style.left = `${position}%`;
            ball.title = situation.downDistanceText || situation.shortDownDistanceText || "Ball Position";

            setVisible(ball, true);
        }

        if (gain) {
            if (gainPosition !== null) {
                gain.style.left = `${gainPosition}%`;

                setVisible(gain, true);
            } else {
                setVisible(gain, false);
            }
        }

        setText(field, "[data-football-possession]", getPossessionName(field, possession));
        setText(field, "[data-football-down]", situation.downDistanceText || situation.shortDownDistanceText || "-");
        setText(field, "[data-football-yards]", yardsToGoal);

        setVisible(redZone, situation.isRedZone === true);
    }

    function renderPlay(container, index) {
        const data = getPlaybackData(container);

        if (!data || data.plays.length === 0) {
            return;
        }

        data.index = clamp(index, 0, data.plays.length - 1);

        const play = data.plays[data.index];

        const field = container.closest("[data-football-field]");

        if (!field) {
            return;
        }

        const quarter = play.quarter ? `Q${play.quarter}` : "Quarter Unknown";

        setText(container, "[data-playback-clock]", `${quarter} - ${play.clock || "-"}`);
        setText(container, "[data-playback-count]", `Play ${data.index + 1} of ${data.plays.length}`);
        setText(container, "[data-playback-drive]", `Drive ${play.driveNumber}`);
        setText(container, "[data-playback-description]", play.description);

        const away = field.dataset.awayAbbreviation;
        const home = field.dataset.homeAbbreviation;

        setText(container, "[data-playback-score]", `${away}: ${play.awayScore ?? "-"} | ${home}: ${play.homeScore ?? "-"}`);

        const progress = container.querySelector("[data-playback-progress]");

        if (progress) {
            progress.style.width = `${(data.index + 1) / data.plays.length * 100}%`;
        }

        if (play.situation) {
            data.lastSituation = play.situation;

            updateField(field, play.situation);

            setText(container, "[data-playback-status]", play.isScoringPlay ? "Scoring Play" : "Showing Field Position at the Start of This Play");
        } else {
            updateField(field, null);

            setText(container, "[data-playback-status]", "Field Position Unavailable for This Play");
        }

        for (const button of container.querySelectorAll("[data-playback-action]")) {
            const action = button.dataset.playbackAction;

            button.disabled = (data.index === 0 && (action === "first" || action === "previous")) || (data.index === data.plays.length - 1 && (action === "next" || action === "last"));
        }
    }

    function handlePlaybackClick(event) {
        const button = event.target.closest("[data-playback-action]");

        if (!button) {
            return;
        }

        const container = button.closest("[data-football-playback]");

        if (!container) {
            return;
        }

        const data = getPlaybackData(container);

        if (!data) {
            return;
        }

        let nextIndex = data.index;

        switch (button.dataset.playbackAction) {
            case "first":
                nextIndex = 0;
                break;

            case "previous":
                nextIndex--;
                break;

            case "next":
                nextIndex++;
                break;

            case "last":
                nextIndex = data.plays.length - 1;
                break;

            default:
                return;
        }

        renderPlay(container, nextIndex);
    }

    document.addEventListener("click", handlePlaybackClick);

    function initializePlaybacks(root = document) {
        const containers = root.querySelectorAll("[data-football-playback]");

        for (const container of containers) {
            if (container.dataset.playbackInitialized === "true") {
                continue;
            }

            container.dataset.playbackInitialized = "true";

            const data = getPlaybackData(container);

            if (data) {
                renderPlay(container, data.index);
            }
        }
    }

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", () => initializePlaybacks());
    } else {
        initializePlaybacks();
    }

    window.SportsTrackerFootballPlayback = {
        initialize: initializePlaybacks,

        captureState(root = document) {
            const container = root.querySelector("[data-football-playback]");

            if (!container) {
                return null;
            }

            const data = getPlaybackData(container);

            if (!data || data.plays.length === 0) {
                return null;
            }

            return {
                playId: data.plays[data.index]?.id ?? null,
                index: data.index
            };
        },

        restoreState(state, root = document) {
            if (!state) {
                return;
            }

            const container = root.querySelector("[data-football-playback]");

            if (!container) {
                return;
            }

            const data = getPlaybackData(container);

            if (!data || data.plays.length === 0) {
                return;
            }

            let index = -1;

            if (state.playId != null) {
                index = data.plays.findIndex(play => play.id === state.playId);
            }

            if (index < 0) {
                index = clamp(state.index, 0, data.plays.length - 1);
            }

            renderPlay(container, index);
        }
    };
})();