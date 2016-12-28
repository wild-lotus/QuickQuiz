using Zenject;

namespace CgfGames {

    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<GameCtrl> ()
                .AsSingle ();

            Container.BindFactory<IMainScreenCtrl, IMainScreenCtrlFactory> ()
                .To<MainScreenCtrl> ();

            Container.BindFactory<IQuizCtrl, IQuizCtrlFactory> ()
                .To<QuizCtrl> ();

            Container.Bind<IQuizRepo> ()
                .To<QuizRepo> ()
                .AsSingle ();

            Container.Bind<IQuizStateDataStore> ()
                .To<QuizStateFileDataStore> ()
                .AsSingle ();

            Container.Bind<IQuestionDataStore> ()
                .To<QuestionRestDataStore> ()
                .AsSingle ();

            Container.BindFactory<IGetQuestionsRequest, IGetQuestionsRequestFactory> ()
                .To<GetQuestionsRequest> ();

            Container.Bind<ILeaderboardDataStore> ()
                .To<LeaderboardRestDataStore> ()
                .AsSingle ();

            Container.BindFactory<IGetLeaderboardPlayerEntryRequest, IGetLeaderboardPlayerEntryRequestFactory> ()
                .To<GetLeaderboardPlayerEntryRequest> ();

            Container.BindFactory<IGetLeaderboardTopEntriesRequest, IGetLeaderboardTopEntriesRequestFactory> ()
                .To<GetLeaderboardTopEntriesRequest> ();

            Container.Bind<LeaderbarodEntryMapper> ()
                .AsSingle ();

            Container.BindFactory<ISetLeaderboardPlayerScoreRequest, ISetLeaderboardPlayerScoreRequestFactory> ()
                .To<SetLeaderboardPlayerScoreRequest> ();

            Container.Bind<IQuizProgressCtrl> ()
                .To<QuizProgressCtrl> ()
                .AsSingle ();

            Container.BindFactory<IQuestion, IQuestionCtrl, IQuestionCtrlFactory> ()
                .To<QuestionCtrl> ();

            Container.BindFactory<ILeaderboardCtrl, ILeaderboardCtrlFactory> ()
                .To<LeaderboardCtrl> ();
        }
    }
}